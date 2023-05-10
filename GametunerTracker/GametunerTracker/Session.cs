/*
 * Session.cs
 * SnowplowTracker
 * 
 * Copyright (c) 2015 Snowplow Analytics Ltd. All rights reserved.
 *
 * This program is licensed to you under the Apache License Version 2.0,
 * and you may not use this file except in compliance with the Apache License Version 2.0.
 * You may obtain a copy of the Apache License Version 2.0 at http://www.apache.org/licenses/LICENSE-2.0.
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the Apache License Version 2.0 is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the Apache License Version 2.0 for the specific language governing permissions and limitations there under.
 * 
 * Authors: Joshua Beemster, Paul Boocock
 * Copyright: Copyright (c) 2015-2019 Snowplow Analytics Ltd
 * License: Apache License Version 2.0
 */

using System;
using System.Collections.Generic;
using System.Threading;
using GametunerTracker.Enums;
using GametunerTracker.Payloads.Contexts;
using GametunerTracker.Logging;
using UnityEngine;

namespace GametunerTracker
{
    internal class Session
    {

        private string SESSION_DEFAULT_PATH = "snowplow_session.dict";

        private SessionContext sessionContext;

        private long sessionTimeout;
        private string currentSessionId;
        private string previousSessionId;
        private int sessionIndex;
        private float startAppTick;
        private float lastActivityTick;
        private string SessionPath;
        private readonly StorageMechanism sessionStorage = StorageMechanism.Litedb;
        public delegate void OnSessionStart(string launchMode);
        public delegate void OnSessionEnd();
        public OnSessionStart onSessionStart;        
        public OnSessionEnd onSessionEnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnowplowTracker.Session"/> class.
        /// </summary>
        /// <param name="foregroundTimeout">Foreground timeout.</param>
        /// <param name="backgroundTimeout">Background timeout.</param>
        /// <param name="checkInterval">Check interval.</param>
        public Session(string sessionPath, long sessionTimeout = 300)
        {
            this.sessionTimeout = sessionTimeout * 1000;

            SessionPath = $"{Application.persistentDataPath }/{sessionPath ?? SESSION_DEFAULT_PATH}";

            Dictionary<string, object> maybeSessionDict = Utils.ReadDictionaryFromFile(SessionPath);
            if (maybeSessionDict == null)
            {
                this.currentSessionId = null;
            }
            else
            {
                if (maybeSessionDict.TryGetValue(Constants.SESSION_ID, out var sessionId))
                {
                    this.currentSessionId = (string)sessionId;
                }
                if (maybeSessionDict.TryGetValue(Constants.SESSION_INDEX, out var sessionIndex))
                {
                    this.sessionIndex = (int)sessionIndex;
                }
            }

            StartNewSession(); 
            UserActivity.OnUserActivity += CheckNewSession;          
        }

        /// <summary>
        /// Invokes the session start.
        /// </summary>
        private void DelegateSessionStart(string launchMode)
        {
            if (onSessionStart != null)
            {
                onSessionStart(launchMode);
            }
        }

        /// <summary>
        /// Invokes the session end.
        /// </summary>
        private void DelegateSessionEnd()
        {
            if (onSessionEnd != null)
            {
                onSessionEnd();
            }
        }

        // --- Public

        /// <summary>
        /// Gets the session context.
        /// </summary>
        /// <returns>The session context.</returns>
        public SessionContext GetSessionContext()
        {
            return new SessionContext()
                    .SetSessionId(currentSessionId)
                    .SetSessionIndex(sessionIndex)
                    .SetSessionTime(GetSessionTime())
                    .Build();
        }

        public void CheckNewSession(long millisecondsSinceLastActivity) {
            Log.Debug("Checking new session. Last activity: " + millisecondsSinceLastActivity + " Session timeout: " + sessionTimeout);

            if (millisecondsSinceLastActivity > sessionTimeout)
            {
                DelegateSessionEnd();
                StartNewSession();
                DelegateSessionStart(Constants.LOGIN_LAUNCH_MODE_SESSION_TIMEOUT);
            }
        }

        /// <summary>
        /// Gets session index.
        /// </summary>
        /// <returns>Session index</returns>
        public int GetSessionIndex() { 
            return this.sessionIndex;
        }

        /// <summary>
        /// Gets previous session id.
        /// </summary>
        /// <returns>Session ID</returns>
        public string GetPreviousSession() { 
            return this.previousSessionId;
        }

        /// <summary>
        /// Gets current session id.
        /// </summary>
        /// <returns>Sessin id</returns>
        public string GetSessionID() {
            return currentSessionId;
        }

        /// <summary>
        /// Gets session time from Init.
        /// </summary>
        /// <returns>Session time in seconds</returns>
        public float GetSessionTime() { 
            return lastActivityTick - startAppTick;
        }

        /// <summary>
        /// Sets last activity tick.
        /// </summary>
        /// <param name="timeSinceStart">Time since app start</param>
        public void SetLastActivityTick(float timeSinceStart){
            this.lastActivityTick = timeSinceStart;
        }

        // --- Private

        /// <summary>
        /// Set data for new session.
        /// </summary>
        private void StartNewSession() { 
            UpdateSession();
            UserActivity.UpdateLastActivityTimestamp(UnityUtils.GetTimeSinceStartupInt());
            UpdateTimeFromStart(UnityUtils.GetTimeSinceStartup());
            UpdateSessionDict();
            Utils.WriteDictionaryToFile(SessionPath, sessionContext.GetData());
        }

        /// <summary>
        /// Updates the session. Session is updated on app start and after timer timeout.
        /// </summary>
        private void UpdateSession()
        {
            previousSessionId = currentSessionId;
            currentSessionId = Utils.GetGUID();
            sessionIndex++;
        }

        /// <summary>
        /// Updates the session dict.
        /// </summary>
        private void UpdateSessionDict()
        {
            SessionContext newSessionContext = new SessionContext()
                    .SetSessionId(currentSessionId)
                    .SetSessionIndex(sessionIndex)
                    .SetSessionTime(GetSessionTime())
                    .Build();
            sessionContext = newSessionContext;
        }

        /// <summary>
        /// Sets time from start.
        /// </summary>
        /// <param name="timeSinceStart"></param>
        private void UpdateTimeFromStart(float timeSinceStart){
            this.startAppTick = timeSinceStart;
            this.lastActivityTick = timeSinceStart;
        }

        
    }
}

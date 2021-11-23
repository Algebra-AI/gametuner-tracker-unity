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
using SnowplowTracker.Enums;
using SnowplowTracker.Payloads.Contexts;
using UnityEngine;

namespace SnowplowTracker
{
    internal class Session
    {

        private string SESSION_DEFAULT_PATH = "snowplow_session.dict";

        private SessionContext sessionContext;

        private long foregroundTimeout;
        private long backgroundTimeout;
        private long checkInterval;
        private long accessedLast;
        private long backgroundAccessed;
        private bool background;
        private string firstEventId;
        private string userId;
        private string currentSessionId;
        private string previousSessionId;
        private int sessionIndex;
        private Timer sessionCheckTimer;
        private string SessionPath;
        private readonly StorageMechanism sessionStorage = StorageMechanism.Litedb;
        public delegate void OnSessionStart();
        public delegate void OnSessionEnd();
        public OnSessionStart onSessionStart;        
        public OnSessionEnd onSessionEnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnowplowTracker.Session"/> class.
        /// </summary>
        /// <param name="foregroundTimeout">Foreground timeout.</param>
        /// <param name="backgroundTimeout">Background timeout.</param>
        /// <param name="checkInterval">Check interval.</param>
        public Session(string sessionPath, long foregroundTimeout = 600, long backgroundTimeout = 300, long checkInterval = 15)
        {
            this.foregroundTimeout = foregroundTimeout * 1000;
            this.backgroundTimeout = backgroundTimeout * 1000;
            this.checkInterval = checkInterval;

            SessionPath = $"{Application.persistentDataPath }/{sessionPath ?? SESSION_DEFAULT_PATH}";

            Dictionary<string, object> maybeSessionDict = Utils.ReadDictionaryFromFile(SessionPath);
            if (maybeSessionDict == null)
            {
                this.userId = Utils.GetGUID();
                this.currentSessionId = null;
            }
            else
            {
                if (maybeSessionDict.TryGetValue(Constants.SESSION_USER_ID, out var userId))
                {
                    this.userId = (string)userId;
                }
                if (maybeSessionDict.TryGetValue(Constants.SESSION_ID, out var sessionId))
                {
                    this.currentSessionId = (string)sessionId;
                }
                if (maybeSessionDict.TryGetValue(Constants.SESSION_PREVIOUS_ID, out var previousId))
                {
                    this.previousSessionId = (string)previousId;
                }
                if (maybeSessionDict.TryGetValue(Constants.SESSION_INDEX, out var sessionIndex))
                {
                    this.sessionIndex = (int)sessionIndex;
                }
            }

            UpdateSession();
            UpdateAccessedLast();
            UpdateSessionDict();
            DelegateSessionStart();
            Utils.WriteDictionaryToFile(SessionPath, sessionContext.GetData());
        }

        /// <summary>
        /// Invokes the session start.
        /// </summary>
        private void DelegateSessionStart()
        {
            if (onSessionStart != null)
            {
                onSessionStart();
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
        /// <param name="eventId">Event identifier.</param>
        public SessionContext GetSessionContext(string eventId)
        {
            UpdateAccessedLast();
            if (firstEventId == null)
            {
                firstEventId = eventId;
                sessionContext.SetFirstEventId(eventId);
                sessionContext.Build();
            }
            Log.Verbose("Session: data: " + Utils.DictToJSONString(sessionContext.GetData()));
            return sessionContext;
        }

        /// <summary>
        /// Starts the session checker.
        /// </summary>
        public void StartChecker()
        {
            if (sessionCheckTimer == null)
            {
                sessionCheckTimer = new Timer(CheckSession, null, checkInterval * 1000, Timeout.Infinite);
            }
        }

        /// <summary>
        /// Stops the session checker.
        /// </summary>
        public void StopChecker()
        {
            if (sessionCheckTimer != null)
            {
                sessionCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);
                sessionCheckTimer.Dispose();
                sessionCheckTimer = null;
            }
        }

        /// <summary>
        /// Sets the foreground timeout seconds.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        public void SetForegroundTimeoutSeconds(long timeout)
        {
            this.foregroundTimeout = timeout * 1000;
        }

        /// <summary>
        /// Sets the background timeout seconds.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        public void SetBackgroundTimeoutSeconds(long timeout)
        {
            this.backgroundTimeout = timeout * 1000;
        }

        /// <summary>
        /// Sets the check interval seconds.
        /// </summary>
        /// <param name="interval">Interval.</param>
        public void SetCheckIntervalSeconds(long interval)
        {
            this.checkInterval = interval;
        }

        /// <summary>
        /// Sets the background truth.
        /// </summary>
        /// <param name="truth">If set to <c>true</c> truth.</param>
        public void SetBackground(bool truth)
        {
            this.background = truth;
            if (truth) {
                GoToBackground();
            } else {
                GoToForeground();
            }
        }

        /// <summary>
        /// Set time when the app was last accessed.
        /// </summary>
        private void GoToBackground()
        {
            backgroundAccessed = Utils.GetTimestamp();
        }  

        /// <summary>
        /// Returns from backgorund.
        /// </summary>
        private void GoToForeground()
        {
            CheckNewSession();
            StartChecker();
        } 

        /// <summary>
        /// Checks is new session is triggered.
        /// </summary>
        private void CheckNewSession() {
            if (backgroundAccessed == 0) {
                return;
            }

            long checkTime = Utils.GetTimestamp();

            if (!Utils.IsTimeInRange(backgroundAccessed, checkTime, backgroundTimeout))
            {
                DelegateSessionStart();
            }
        }

        /// <summary>
        /// Gets the foreground timeout.
        /// </summary>
        /// <returns>The foreground timeout.</returns>
        public long GetForegroundTimeout()
        {
            return this.foregroundTimeout / 1000;
        }

        /// <summary>
        /// Gets the background timeout.
        /// </summary>
        /// <returns>The background timeout.</returns>
        public long GetBackgroundTimeout()
        {
            return this.backgroundTimeout / 1000;
        }

        /// <summary>
        /// Gets the check interval.
        /// </summary>
        /// <returns>The check interval.</returns>
        public long GetCheckInterval()
        {
            return this.checkInterval;
        }

        /// <summary>
        /// Gets the background state.
        /// </summary>
        /// <returns><c>true</c>, if background was gotten, <c>false</c> otherwise.</returns>
        public bool GetBackground()
        {
            return this.background;
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

        // --- Private

        /// <summary>
        /// Checks the session.
        /// </summary>
        private void CheckSession(object state)
        {
            Log.Verbose("Session: Checking session...");

            long checkTime = Utils.GetTimestamp();
            long range = background ? backgroundTimeout : foregroundTimeout;
            long accessedTime = background ? backgroundAccessed : accessedLast;

            if (!Utils.IsTimeInRange(accessedTime, checkTime, range))
            {
                Log.Debug("Session: Session expired; resetting session.");
                try
                {
                    DelegateSessionEnd();                
                    UpdateSession();
                    UpdateAccessedLast();
                    UpdateSessionDict();                
                    Utils.WriteDictionaryToFile(SessionPath, sessionContext.GetData());
                    if (background) {
                        StopChecker();
                        return;
                    }                     
                    DelegateSessionStart();
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
                
            }

            sessionCheckTimer.Change(checkInterval * 1000, Timeout.Infinite);
        }

        /// <summary>
        /// Updates the session. Session is updated on app start and after timer timeout.
        /// </summary>
        private void UpdateSession()
        {
            previousSessionId = currentSessionId;
            currentSessionId = Utils.GetGUID();
            sessionIndex++;
            firstEventId = null;
        }

        /// <summary>
        /// Updates the accessed last.
        /// </summary>
        private void UpdateAccessedLast()
        {
            accessedLast = Utils.GetTimestamp();
        }

        /// <summary>
        /// Updates the session dict.
        /// </summary>
        private void UpdateSessionDict()
        {
            SessionContext newSessionContext = new SessionContext()
                    .SetUserId(userId)
                    .SetSessionId(currentSessionId)
                    .SetPreviousSessionId(previousSessionId)
                    .SetSessionIndex(sessionIndex)
                    .SetStorageMechanism(sessionStorage)
                    .Build();
            sessionContext = newSessionContext;
        }
    }
}

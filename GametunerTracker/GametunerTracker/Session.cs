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
using SnowplowTracker.Logging;
using GametunerTracker;
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
        private long backgroundAccessedTimestamp;
        private float backgroundAccessedTimeFromStart;
        private bool background;
        private string firstEventId;
        private string userId;
        private string currentSessionId;
        private string previousSessionId;
        private int sessionIndex;
        private float startAppTick;
        private float lastActivityTick;
        private Timer sessionCheckTimer;
        private string SessionPath;
        private readonly StorageMechanism sessionStorage = StorageMechanism.Litedb;
        public delegate void OnSessionStart();
        public delegate void OnSessionEnd(EventData eventData);
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
            UpdateTimeFromStart(UnityUtils.GetTimeSinceStartup());

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

            StartNewSession();
            DelegateSessionStart();            
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
        private void DelegateSessionEnd(EventData eventData)
        {
            if (onSessionEnd != null)
            {
                onSessionEnd(eventData);
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
            backgroundAccessedTimestamp = Utils.GetTimestamp();
            backgroundAccessedTimeFromStart = UnityUtils.GetTimeSinceStartup();
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
            if (backgroundAccessedTimestamp == 0) {
                return;
            }

            long checkTime = Utils.GetTimestamp();

            if (!Utils.IsTimeInRange(backgroundAccessedTimestamp, checkTime, backgroundTimeout))
            {
                DelegateSessionEnd(GetSessionEndEventData(true));
                StartNewSession();
                DelegateSessionStart();
            }
        }

        /// <summary>
        /// Returns the session end timestamp.
        /// </summary>
        /// <returns>Event data of session end</returns>
        private EventData GetSessionEndEventData(bool returnFromBackground)
        {
            EventData eventData = new EventData();
            eventData.EventTimestamp = 0;            
            eventData.EventSessionTime = 0;

            if (returnFromBackground) {       
                eventData.EventTimestamp = this.backgroundAccessedTimestamp + this.backgroundTimeout;
                eventData.EventSessionTime = this.backgroundAccessedTimeFromStart + (this.backgroundTimeout / 1000) - this.startAppTick;                
            } 

            return eventData;
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
        /// Checks the session in foreground.
        /// </summary>
        private void CheckSession(object state)
        {
            if (background) {
                return;
            }

            Log.Verbose("Session: Checking session...");

            long checkTime = Utils.GetTimestamp();

            if (!Utils.IsTimeInRange(accessedLast, checkTime, foregroundTimeout))
            {
                Log.Debug("Session: Session expired; resetting session.");
                try
                {
                    DelegateSessionEnd(GetSessionEndEventData(false));                
                    StartNewSession();             
                    DelegateSessionStart();
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            }

            if(sessionCheckTimer != null) {
                sessionCheckTimer.Change(checkInterval * 1000, Timeout.Infinite);
            }
        }


        /// <summary>
        /// Set data for new session.
        /// </summary>
        private void StartNewSession() { 
            UpdateSession();
            UpdateAccessedLast();
            UpdateSessionDict();
            UpdateTimeFromStart(UnityUtils.GetTimeSinceStartup());
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

        /// <summary>
        /// Sets time from start.
        /// </summary>
        /// <param name="timeSinceStart"></param>
        private void UpdateTimeFromStart(float timeSinceStart){
            this.startAppTick = this.lastActivityTick = timeSinceStart;
        }

        
    }
}

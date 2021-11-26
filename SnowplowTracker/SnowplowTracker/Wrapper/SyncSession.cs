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
using System.Collections;
using System.Collections.Generic;
using SnowplowTracker.Enums;
using SnowplowTracker.Payloads.Contexts;
using SnowplowTracker.Wrapper;
using UnityEngine;

namespace SnowplowTracker
{
    internal class SyncSession : Session
    {
        private System.Collections.IEnumerator sessionChecker;

        public SyncSession(string sessionPath, long foregroundTimeout = 600, long backgroundTimeout = 300, long checkInterval = 15) : base(sessionPath, foregroundTimeout, backgroundTimeout, checkInterval)
        {
            
        }

        /// <summary>
        /// Starts the session checker.
        /// </summary>
        public override void StartChecker()
        {
            if (sessionChecker == null)
            {
                sessionChecker = CheckingSession();
                UnityMainThreadDispatcher.Instance.StartCoroutine(sessionChecker);
            }
        }

        private IEnumerator CheckingSession() {
            while (true)
            {
                yield return new WaitForSeconds(checkInterval);
                CheckSession(null);
            }           
        }

        /// <summary>
        /// Checks the session.
        /// </summary>
        protected override void CheckSession(object state)
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
        }

        /// <summary>
        /// Stops the session checker.
        /// </summary>
        public override void StopChecker()
        {
            if (UnityMainThreadDispatcher.Instance != null && sessionChecker != null) { 
                UnityMainThreadDispatcher.Instance.StopCoroutine(sessionChecker);
                sessionChecker = null;
            }            
        }
    }
}
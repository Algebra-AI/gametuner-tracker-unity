/*
 * SessionContext.cs
 * SnowplowTracker.Payloads.Contexts
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

/*
 * Modified by AlgebraAI on 2024-01-31
 * - Change session context fields
 */

using GametunerTracker.Enums;

namespace GametunerTracker.Payloads.Contexts
{
    internal class SessionContext : AbstractContext<SessionContext> {

		/// <summary>
		/// Sets the session identifier.
		/// </summary>
		/// <returns>The session identifier.</returns>
		/// <param name="sessionId">Session identifier.</param>
		public SessionContext SetSessionId(string sessionId) {
			this.DoAdd (Constants.SESSION_ID, sessionId);
			return this;
		}

		/// <summary>
		/// Sets the index of the session.
		/// </summary>
		/// <returns>The session index.</returns>
		/// <param name="sessionIndex">Session index.</param>
		public SessionContext SetSessionIndex(int sessionIndex) {
			this.DoAdd (Constants.SESSION_INDEX, sessionIndex);
			return this;
		}

		/// <summary>
		/// Sets the time of the session.
		/// </summary>
		/// <returns>The session time.</returns>
		/// <param name="sessionTime">Session time.</param>
		public SessionContext SetSessionTime(float sessionTime) {
			this.DoAdd (Constants.SESSION_TIME, sessionTime);
			return this;
		}
		
		public override SessionContext Build() {
			Utils.CheckArgument (this.data.ContainsKey(Constants.SESSION_ID), "Session Context requires 'session_id'.");
			Utils.CheckArgument (this.data.ContainsKey(Constants.SESSION_INDEX), "Session Context requires 'session_index'.");
			Utils.CheckArgument (this.data.ContainsKey(Constants.SESSION_TIME), "Session Context requires 'session_time'.");
			this.schema = Constants.SCHEMA_SESSION_CONTEXT;
			this.context = new SelfDescribingJson (this.schema, this.data);
			return this;
		}
	}
}

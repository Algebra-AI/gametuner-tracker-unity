/*
 * EventBuilder.cs
 * SnowplowTracker.Events
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
 * - Added priority and eventIndex to AbstractEvent
 */

using System;
using System.Collections.Generic;
using GametunerTracker.Payloads;
using GametunerTracker.Payloads.Contexts;

namespace GametunerTracker.Events
{
    internal abstract class AbstractEvent<T> : IEvent {

		protected List<IContext> customContexts = new List<IContext>();
		protected long timestamp = Utils.GetTimestamp();
		protected string eventId = Utils.GetGUID();
        protected int priority = 0;
        protected int eventIndex = 0;

        public abstract T Self ();
		public abstract T Build ();

		// --- Builder Methods

		/// <summary>
		/// Sets the custom context list for the event.
		/// </summary>
		/// <returns>The custom context.</returns>
		/// <param name="customContexts">Custom contexts.</param>
		public T SetCustomContext(List<IContext> customContexts) {
			if (customContexts != null) {
				this.customContexts = customContexts;
			}
			return Self ();
		}

		/// <summary>
		/// Sets the timestamp (in ms since unix epoch).
		/// </summary>
		/// <returns>The timestamp.</returns>
		/// <param name="timestamp">Timestamp.</param>
		public T SetTimestamp(long timestamp) {
			this.timestamp = timestamp;
			return Self ();
		}

		/// <summary>
		/// Sets the event identifier (as a GUID).
		/// </summary>
		/// <returns>The event identifier.</returns>
		/// <param name="eventId">Event identifier.</param>
		public T SetEventId(string eventId) {
			if (!String.IsNullOrEmpty (eventId)) {
				this.eventId = eventId;
			}
			return Self ();
		}
		
		/// <summary>
		/// Sets the event proprity.
		/// </summary>
		/// <returns>The event proprity.</returns>
		/// <param name="eventId">Event proprity.</param>
		public T SetEventPriority(int priority) {
			this.priority = priority;
			return Self ();
		}

		/// <summary>
		/// Sets the event index.
		/// </summary>
		/// <returns>The event index.</returns>
		/// <param name="eventId">Event index.</param>
		public T SetEventIndex(int eventIndex) {
			this.eventIndex = eventIndex;
			return Self ();
		}

		// --- Helper Methods

		/// <summary>
		/// Adds the common key-value pairs to an event payload
		/// </summary>
		/// <param name="payload">The payload to append values to</param>
		/// <returns>The complete payload</returns>
		protected TrackerPayload AddDefaultPairs(TrackerPayload payload) {
			payload.Add (Constants.EID, eventId);
			payload.Add (Constants.TIMESTAMP, timestamp.ToString());
			return payload;
		}

		// --- Interface Methods

		/// <summary>
		/// Gets the list of custom contexts attached to the event.
		/// </summary>
		/// <returns>The custom contexts list</returns>
		public List<IContext> GetContexts() {
			return customContexts;
		}

		/// <summary>
		/// Gets the event timestamp that has been set.
		/// </summary>
		/// <returns>The event timestamp</returns>
		public long GetTimestamp() {
			return timestamp;
		}

		/// <summary>
		/// Gets the event GUID that has been set.
		/// </summary>
		/// <returns>The event guid</returns>
		public  string GetEventId() {
			return eventId;
		}

		/// <summary>
		/// Gets the event priority that has been set.
		/// </summary>
		/// <returns>The event priority</returns>
		public int GetEventPriority() {
			return priority;
		}

		/// <summary>
		/// Gets the event index that has been set.
		/// </summary>
		/// <returns>The event index</returns>
		public int GetEventIndex() {
			return eventIndex;
		}

		/// <summary>
		/// Gets the event payload.
		/// </summary>
		/// <returns>The event payload</returns>
		public abstract IPayload GetPayload ();
	}
}

/*
 * EventContext.cs
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

using System;
using SnowplowTracker.Enums;

namespace SnowplowTracker.Payloads.Contexts
{
	/// <summary>
	/// Event context.
	/// </summary>
    internal class EventContext : AbstractContext<EventContext> {

        /// <summary>
        /// Sets event index
        /// </summary>
        /// <param name="eventIndex">Event index</param>
        /// <returns>EventContext</returns>
        public EventContext SetEventIndex(int eventIndex) {
			this.DoAdd(Constants.EVENT_INDEX, eventIndex);
			return this;
		}

        /// <summary>
        /// Sets previous event name
        /// </summary>
        /// <param name="previousEvent">Previous event name</param>
        /// <returns>EventContext</returns>
        public EventContext SetPreviousEventName(string previousEvent) {
			this.DoAdd(Constants.EVENT_PREVIOUS_EVENT, previousEvent);
			return this;
		}

        /// <summary>
        /// Sets transaction ID
        /// </summary>
        /// <param name="transactionID">Transaction ID</param>
        /// <returns></returns>
        public EventContext SetTransactionID(string transactionID) {
			this.DoAdd(Constants.EVENT_TRANSACTION_ID, transactionID);
			return this;
		}    
		
		public override EventContext Build() {
			this.schema = Constants.SCHEMA_EVENT_CONTEXT;
			this.context = new SelfDescribingJson (this.schema, this.data);
			return this;
		}

    }
}

/*
 * IEvent.cs
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

using System.Collections.Generic;
using GametunerTracker.Payloads;
using GametunerTracker.Payloads.Contexts;

namespace GametunerTracker.Events
{
    internal interface IEvent {

		/// <summary>
		/// Gets the list of custom contexts attached to the event.
		/// </summary>
		/// <returns>The custom contexts list</returns>
		List<IContext> GetContexts();

		/// <summary>
		/// Gets the event timestamp that has been set.
		/// </summary>
		/// <returns>The event timestamp</returns>
		long GetTimestamp();

		/// <summary>
		/// Gets the event GUID that has been set.
		/// </summary>
		/// <returns>The event guid</returns>
		string GetEventId();

		/// <summary>
		/// Gets the event payload.
		/// </summary>
		/// <returns>The event payload</returns>
		IPayload GetPayload();
    }
}

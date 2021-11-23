/*
 * EventNames.cs
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

namespace SnowplowTracker {
	public class EventNames {

		// Events
		public readonly static string EVENT_REGISTRATION    = "registration";
		public readonly static string EVENT_LOGIN           = "login";
		public readonly static string EVENT_LOGOUT          = "logout";
		public readonly static string EVENT_CURRENCY_CHANGE = "currency_change";
		public readonly static string EVENT_PURCHASE        = "purchase";
		public readonly static string EVENT_AD_WATCHED      = "ad_watched";
		public readonly static string EVENT_LEVEL_STARTED   = "level_started";
		public readonly static string EVENT_LEVEL_PLAYED    = "level_played";
		public readonly static string EVENT_USER_STATE      = "user_state";
		public readonly static string EVENT_EVENT_STARTED   = "event_started";
		public readonly static string EVENT_EVENT_ENDED     = "event_ended";
		public readonly static string EVENT_ERROR           = "error";
		public readonly static string EVENT_SHOW_POPUP      = "show_popup";
		public readonly static string EVENT_NOTIFICATION    = "notification";
	}
}

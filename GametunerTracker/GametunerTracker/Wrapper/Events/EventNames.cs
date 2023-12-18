/*
 * EventNames.cs
 * GametunerTracker
 * 
 * Copyright (c) 2024 AlgebraAI All rights reserved.
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
 * Authors: Djordje Smiljanic
 * Copyright: Copyright (c) 2022-2024 AlgebraAI
 * License: Apache License Version 2.0
 */

using System;
using System.Collections.Generic;
using GametunerTracker;

namespace GametunerTracker {
	internal class EventNames {

        public static List<StandardEvent> standardEvents = new List<StandardEvent>() { 
			new StandardEvent(EVENT_FIRST_OPEN, Constants.EVENT_FIRST_OPEN_SCHEMA),
			new StandardEvent(EVENT_LOGIN, Constants.EVENT_LOGIN_SCHEMA),
			new StandardEvent(EVENT_LOGOUT, Constants.EVENT_LOGOUT_SCHEMA)
		};
        // Events
        public const string EVENT_FIRST_OPEN   			= "first_open";
		public const string EVENT_LOGIN         		= "login";
		public const string EVENT_LOGOUT         		= "logout";
		public const string EVENT_CURRENCY_CHANGE 		= "currency_change";
		public const string EVENT_PURCHASE        		= "purchase";
		public const string EVENT_PURCHASE_INITIATED    = "purchase_initiated";
		public const string EVENT_AD_WATCHED      		= "ad_watched";
		public const string EVENT_AD_STARTED     		= "ad_started";
		public const string EVENT_LEVEL_STARTED   		= "level_started";
		public const string EVENT_LEVEL_PLAYED    		= "level_played";
		public const string EVENT_USER_STATE      		= "user_state";
		public const string EVENT_EVENT_STARTED   		= "event_started";
		public const string EVENT_EVENT_ENDED     		= "event_ended";
		public const string EVENT_ERROR           		= "error";
		public const string EVENT_SHOW_POPUP      		= "show_popup";
		public const string EVENT_NOTIFICATION    		= "notification";
		public const string EVENT_GDPR_DELETE_REQUEST	= "gdpr_delete_request";

		public static bool IsInStandardEvents(string eventName) {
			foreach (StandardEvent standardEvent in standardEvents) {
				if (standardEvent.EventName.Equals(eventName)) {
					return true;
				}
			}
			return false;
		}

		public static string GetSchema(string eventName) {
			foreach (StandardEvent standardEvent in standardEvents) {
				if (standardEvent.EventName.Equals(eventName)) {
					return standardEvent.EventSchema;
				}
			}
			return null;
		}
	}
}

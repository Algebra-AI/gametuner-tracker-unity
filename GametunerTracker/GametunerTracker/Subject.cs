/*
 * Subject.cs
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

/*
 * Modified by AlgebraAI on 2024-01-31
 * - Added userId, installationId, sandboxMode, apiKey fields
 */

using System;
using System.Collections.Generic;
using GametunerTracker.Payloads;

namespace GametunerTracker {
	internal class Subject {

		private TrackerPayload standardDict;

		/// <summary>
		/// Initializes a new instance of the <see cref="SnowplowTracker.Subject"/> class.
		/// </summary>
		public Subject () {
			standardDict = new TrackerPayload ();
		}

		/// <summary>
		/// Sets the user identifier.
		/// </summary>
		/// <param name="userId">User identifier.</param>
		public void SetUserId(String userId) {
			this.standardDict.Add (Constants.UID, userId);
		}

		/// <summary>
		/// Sets the installation id.
		/// </summary>
		/// <param name="pseudoId">User pseudo id.</param>
		public void SetInstallationId(String installationId) {
			this.standardDict.Add (Constants.INSTALLATION_ID, installationId);
		}	

		/// <summary>
		/// Sets sandbox mode.
		/// </summary>
		/// <param name="sandboxModeEnabled">Is sandbox mode enabled</param>
		public void SetSandboxMode(bool sandboxModeEnabled) {
			this.standardDict.Add (Constants.SANDBOX_MODE, Convert.ToInt32(sandboxModeEnabled).ToString());
		}

		// <summary>
		/// Sets api key.
		/// </summary>
		/// <param name="apiKey">Api key.</param>
		public void SetApiKey(String apiKey) {
			this.standardDict.Add (Constants.API_KEY, apiKey);
		}		

		/// <summary>
		/// Gets the user ID. If ID is not found, returns null.
		/// </summary>
		/// <returns>UserID</returns>
		public string GetUserID() {
            if (this.standardDict.GetDictionary() == null) {
                return null;
            }
            if (this.standardDict.GetDictionary().ContainsKey(Constants.UID))
			{
				if(!string.IsNullOrEmpty(this.standardDict.GetDictionary()[Constants.UID].ToString())) {
					return this.standardDict.GetDictionary()[Constants.UID].ToString();
				}
			}

            return null;
        }

		/// <summary>
		/// Gets installation ID.
		/// </summary>
		/// <returns>Installation ID</returns>
		public string GetInstallationID() {
            if (this.standardDict.GetDictionary() == null) {
                return null;
            }
            if (this.standardDict.GetDictionary().ContainsKey(Constants.INSTALLATION_ID))
			{
				return this.standardDict.GetDictionary()[Constants.INSTALLATION_ID].ToString();
			}

            return null;
        }

		/// <summary>
		/// Sets the screen resolution.
		/// </summary>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public void SetScreenResolution(int width, int height) {
			String res = width.ToString() + "x" + height.ToString();
			this.standardDict.Add (Constants.RESOLUTION, res);
		}

		/// <summary>
		/// Sets the view port.
		/// </summary>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public void SetViewPort(int width, int height) {
			String res = width.ToString() + "x" + height.ToString();
			this.standardDict.Add (Constants.VIEWPORT, res);
		}

		/// <summary>
		/// Sets the color depth.
		/// </summary>
		/// <param name="depth">Depth.</param>
		public void SetColorDepth(int depth) {
			this.standardDict.Add (Constants.COLOR_DEPTH, depth.ToString());
		}

		/// <summary>
		/// Sets the timezone.
		/// </summary>
		/// <param name="timezone">Timezone.</param>
		public void SetTimezone(String timezone) {
			this.standardDict.Add (Constants.TIMEZONE, timezone);
		}
		
		/// <summary>
		/// Sets the language.
		/// </summary>
		/// <param name="language">Language.</param>
		public void SetLanguage(String language) {
			this.standardDict.Add (Constants.LANGUAGE, language);
		}

		/// <summary>
		/// Sets the ip address.
		/// </summary>
		/// <param name="ipAddress">Ip address.</param>
		public void SetIpAddress(String ipAddress) {
			this.standardDict.Add (Constants.IP_ADDRESS, ipAddress);
		}

		/// <summary>
		/// Sets the useragent.
		/// </summary>
		/// <param name="useragent">Useragent.</param>
		public void SetUseragent(String useragent) {
			this.standardDict.Add (Constants.USERAGENT, useragent);
		}

		/// <summary>
		/// Sets the domain user identifier.
		/// </summary>
		/// <param name="domainUserId">Domain user identifier.</param>
		public void SetDomainUserId(String domainUserId) {
			this.standardDict.Add (Constants.DOMAIN_UID, domainUserId);
		}

		/// <summary>
		/// Sets the network user identifier.
		/// </summary>
		/// <param name="networkUserId">Network user identifier.</param>
		public void SetNetworkUserId(String networkUserId) {
			this.standardDict.Add (Constants.NETWORK_UID, networkUserId);
		}

		/// <summary>
		/// Gets the subject.
		/// </summary>
		/// <returns>The subject.</returns>
		public TrackerPayload GetPayload() {
			return this.standardDict;
		}
	}
}

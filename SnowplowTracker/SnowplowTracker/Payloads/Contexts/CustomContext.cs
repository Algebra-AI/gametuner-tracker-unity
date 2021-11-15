/*
 * MobileContext.cs
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

using SnowplowTracker.Enums;

namespace SnowplowTracker.Payloads.Contexts
{
	/// <summary>
	/// TODO - redefinisati custom context ukoliko nam bude trebalo
	/// za sada je samo primer mobile context-a
	/// </summary>
    internal class CustomContext : AbstractContext<CustomContext> {

		/// <summary>
		/// Sets the type of the os.
		/// </summary>
		/// <returns>The os type.</returns>
		/// <param name="osType">Os type.</param>
		public CustomContext SetOsType(string osType) {
			this.DoAdd (Constants.PLAT_OS_TYPE, osType);
			return this;
		}

		/// <summary>
		/// Sets the os version.
		/// </summary>
		/// <returns>The os version.</returns>
		/// <param name="osVersion">Os version.</param>
		public CustomContext SetOsVersion(string osVersion) {
			this.DoAdd (Constants.PLAT_OS_VERSION, osVersion);
			return this;
		}

		/// <summary>
		/// Sets the device manufacturer.
		/// </summary>
		/// <returns>The device manufacturer.</returns>
		/// <param name="deviceManufacturer">Device manufacturer.</param>
		public CustomContext SetDeviceManufacturer(string deviceManufacturer) {
			this.DoAdd (Constants.PLAT_DEVICE_MANU, deviceManufacturer);
			return this;
		}

		/// <summary>
		/// Sets the device model.
		/// </summary>
		/// <returns>The device model.</returns>
		/// <param name="deviceModel">Device model.</param>
		public CustomContext SetDeviceModel(string deviceModel) {
			this.DoAdd (Constants.PLAT_DEVICE_MODEL, deviceModel);
			return this;
		}
		
		public override CustomContext Build() {
			Utils.CheckArgument (this.data.ContainsKey(Constants.PLAT_OS_TYPE), "MobileContext requires 'osType'.");
			Utils.CheckArgument (this.data.ContainsKey(Constants.PLAT_OS_VERSION), "MobileContext requires 'osVersion'.");
			Utils.CheckArgument (this.data.ContainsKey(Constants.PLAT_DEVICE_MANU), "MobileContext requires 'deviceManufacturer'.");
			Utils.CheckArgument (this.data.ContainsKey(Constants.PLAT_DEVICE_MODEL), "MobileContext requires 'deviceModel'.");
			this.schema = Constants.SCHEMA_MOBILE;
			this.context = new SelfDescribingJson (this.schema, this.data);
			return this;
		}
	}
}

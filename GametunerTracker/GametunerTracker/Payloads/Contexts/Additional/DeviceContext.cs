/*
 * DeviceContext.cs
 * GametunerTracker.Payloads.Contexts
 * 
 * Copyright (c) 2024 AlgebraAI. All rights reserved.
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
 * Author: Djordje Smiljanic
 * Copyright: Copyright (c) 2022-2024 AlgebraAI
 * License: Apache License Version 2.0
 */

using System;
using GametunerTracker.Enums;

namespace GametunerTracker.Payloads.Contexts
{
	/// <summary>
	/// Device data context.
	/// </summary>
    internal class DeviceContext : AbstractContext<DeviceContext> {

		/// <summary>
		/// Sets the device category, mobile/tablet/desktop.
		/// </summary>
		/// <returns>DeviceContext.</returns>
		/// <param name="deviceCategory">The device category.</param>
		public DeviceContext SetDeviceCategory(string deviceCategory) {
			this.DoAdd(Constants.DEVICE_CATEGORY, deviceCategory);
			return this;
		}

		/// <summary>
		/// Sets the device manufacturer.
		/// </summary>
		/// <param name="deviceManufacturer">Device manufacturer</param>
		/// <returns></returns>
		public DeviceContext SetDeviceManufacturer(string deviceManufacturer) {
			this.DoAdd(Constants.DEVICE_MANUFACTURER, deviceManufacturer);
			return this;
		}

		/// <summary>
		/// Sets the device model.
		/// </summary>
		/// <param name="model">Device model name</param>
		/// <returns></returns>
		public DeviceContext SetDeviceModel(string model) {
			this.DoAdd(Constants.DEVICE_MODEL, model);
			return this;
		}

		/// <summary>
		/// Sets the OS version.
		/// </summary>
		/// <param name="osVersion">OS version</param>
		/// <returns></returns>
		public DeviceContext SetOsVersion(string osVersion) {
			this.DoAdd(Constants.DEVICE_OS_VERSION, osVersion);
			return this;
		}

		/// <summary>
		/// Sets CPU type.
		/// </summary>
		/// <param name="cpuType">CPU type</param>
		/// <returns></returns>
		public DeviceContext SetCpuType(string cpuType) {
			this.DoAdd(Constants.DEVICE_CPU_TYPE, cpuType);
			return this;
		}

		/// <summary>
		/// Sets GPU type.
		/// </summary>
		/// <param name="gpu">GPU name</param>
		/// <returns></returns>
		public DeviceContext SetGpu(string gpu) {
			this.DoAdd(Constants.DEVICE_GPU, gpu);
			return this;
		}

		/// <summary>
		/// Sets RAM size.
		/// </summary>
		/// <param name="ramSize">RAM memory size</param>
		/// <returns></returns>
		public DeviceContext SetRamSize(int ramSize) {
			this.DoAdd(Constants.DEVICE_RAM_SIZE, ramSize);
			return this;
		}

		/// <summary>
		/// Sets screen size.
		/// </summary>
		/// <param name="width">Screen width</param>
		/// <param name="height">Screen height</param>
		/// <returns></returns>
		public DeviceContext SetScreenResolution(int width, int height) {
			String res = width.ToString() + "x" + height.ToString();
			this.DoAdd(Constants.DEVICE_SCREEN_RES, res);
			return this;
		}

		/// <summary>
		/// Sets device language.
		/// </summary>
		/// <param name="deviceLanguage">Device language</param>
		/// <returns></returns>
		public DeviceContext SetDeviceLanguage(string deviceLanguage) {
			this.DoAdd (Constants.DEVICE_LANGUAGE, deviceLanguage);
			return this;
		}

		/// <summary>
		/// Sets device timezone.
		/// </summary>
		/// <param name="deviceTimezone">Device timezone in format +-HH:MM</param>
		/// <returns></returns>
		public DeviceContext SetDeviceTimezone(string deviceTimezone) {
			this.DoAdd (Constants.DEVICE_TIMEZONE, deviceTimezone);
			return this;
		}

		/// <summary>
		/// Sets source.
		/// </summary>
		/// <param name="source">Source name</param>
		/// <returns></returns>
		public DeviceContext SetSource(string source) {
			this.DoAdd (Constants.DEVICE_SOURCE, source);
			return this;
		}

		/// <summary>
		/// Sets the medium.
		/// </summary>
		/// <param name="medium">Medium name</param>
		/// <returns></returns>
		public DeviceContext SetMedium(string medium) {
			this.DoAdd (Constants.DEVICE_MEDIUM, medium);
			return this;
		}

		/// <summary>
		/// Sets the campaign.
		/// </summary>
		/// <param name="campaign">Campaign ID</param>
		/// <returns></returns>
		public DeviceContext SetCampaign(string campaign) {
			this.DoAdd (Constants.DEVICE_CAMPAIGN, campaign);
			return this;
		}

		/// <summary>
		/// Sets build version.
		/// </summary>
		/// <param name="buildVersion">Build version</param>
		/// <returns></returns>
		public DeviceContext SetBuildVersion(string buildVersion) {
			this.DoAdd (Constants.DEVICE_BUILD_VERSION, buildVersion);
			return this;
		}

		/// <summary>
		/// Sets device ID.
		/// </summary>
		/// <param name="deviceID">Device ID</param>
		/// <returns></returns>
		public DeviceContext SetDeviceId(string deviceID) {
			this.DoAdd (Constants.DEVICE_DEVICE_ID, deviceID);
			return this;
		}

		/// <summary>
		/// Sets advertising ID.
		/// </summary>
		/// <param name="advertisingID"></param>
		/// <returns></returns>
		public DeviceContext SetAdvertisingID(string advertisingID) {
			this.DoAdd (Constants.DEVICE_ADVERTISING_ID, advertisingID);
			return this;
		}

		/// <summary>
		/// Sets idfa.
		/// </summary>
		/// <param name="idfa">Apple idfa</param>
		/// <returns></returns>
		public DeviceContext SetIDFA(string idfa) {
			this.DoAdd (Constants.DEVICE_IDFA, idfa);
			return this;
		}

		/// <summary>
		/// Sets idfv.
		/// </summary>
		/// <param name="idfv">Apple idfv</param>
		/// <returns></returns>
		public DeviceContext SetIDFV(string idfv) {
			this.DoAdd (Constants.DEVICE_IDFV, idfv);
			return this;
		}

		/// <summary>
		/// Sets is device rooted.
		/// </summary>
		/// <param name="isHacked">Is device rooted.</param>
		/// <returns></returns>
		public DeviceContext SetIsHacked(string isHacked) {
			this.DoAdd (Constants.DEVICE_IS_HACKED, isHacked);
			return this;
		}

		public DeviceContext SetStore(string store) {
			this.DoAdd (Constants.DEVICE_STORE, store);
			return this;
		}
		
		public override DeviceContext Build() {
			this.schema = Constants.SCHEMA_DEVICE_CONTEXT;
			this.context = new SelfDescribingJson (this.schema, this.data);
			return this;
		}
	}
}

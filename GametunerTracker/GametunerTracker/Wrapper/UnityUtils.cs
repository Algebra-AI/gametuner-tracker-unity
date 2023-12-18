/*
 * UnityUtils.cs
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
using System.Globalization;
using GametunerTracker.Enums;
using UnityEngine;

namespace GametunerTracker
{
    /// <summary>
    /// Colect device data using Unity API.
    /// </summary>
    internal class UnityUtils
    {
        /// <summary>
        /// Gets CPU type
        /// </summary>
        /// <returns>CPU type</returns>
        public static string GetCpuType() {
            string cpuType = string.Empty;
            if (CultureInfo.InvariantCulture.CompareInfo.IndexOf(SystemInfo.processorType, "ARM", CompareOptions.IgnoreCase) >= 0)
            {
                if (Environment.Is64BitProcess)
                    cpuType = "ARM64";
                else
                    cpuType = "ARM";
            }
            else
            {
                // Must be in the x86 family.
                if (Environment.Is64BitProcess)
                    cpuType = "x86_64";
                else
                    cpuType = "x86";
            }
            return cpuType;
        }

        /// <summary>
        /// Get build (app) version.
        /// </summary>
        /// <returns>Build version</returns>
        public static string GetBuildVersion() {
            return Application.version;
        }

        /// <summary>
        /// Gets device platform
        /// </summary>
        /// <returns>Device platform</returns>
        public static DevicePlatforms GetDevicePlatform() { 
            DevicePlatforms platform = DevicePlatforms.Mobile;

            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    platform = DevicePlatforms.Android;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    platform = DevicePlatforms.IOS;
                    break;
                case RuntimePlatform.WebGLPlayer:
                    platform = DevicePlatforms.Web;
                    break;
                default:
                    platform = DevicePlatforms.Desktop;
                    break;
            }
            return platform;
        }

        /// <summary>
        /// Gets device platform
        /// </summary>
        /// <returns>Device platform</returns>
        public static string GetRuntimePlatform() { 
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return "android";
                case RuntimePlatform.IPhonePlayer:
                    return "ios";
                case RuntimePlatform.WebGLPlayer:
                    return "webgl";
                default:
                    return "desktop";
            }
        }

        /// <summary>
        /// Gets network type
        /// </summary>
        /// <returns>Network type</returns>
        public static NetworkType GetNetworkType() { 
            NetworkType networkType = NetworkType.Offline;

            switch (Application.internetReachability) { 
                case NetworkReachability.NotReachable:
                    networkType = NetworkType.Offline;
                    break;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    networkType = NetworkType.Mobile;
                    break;
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    networkType = NetworkType.Wifi;
                    break;
                default:
                    break;
            }

            return networkType;
        }

        /// <summary>
        /// Gets device language in ISO 639-1 format
        /// </summary>
        /// <returns>Device language code</returns>
        public static string GetDeviceLanguage() {
            SystemLanguage lang = Application.systemLanguage;
            string res = "en";
            switch (lang) {
                case SystemLanguage.Afrikaans: res = "af"; break;
                case SystemLanguage.Arabic: res = "ar"; break;
                case SystemLanguage.Basque: res = "eu"; break;
                case SystemLanguage.Belarusian: res = "by"; break;
                case SystemLanguage.Bulgarian: res = "bg"; break;
                case SystemLanguage.Catalan: res = "ca"; break;
                case SystemLanguage.Chinese: res = "zh"; break;
                case SystemLanguage.Czech: res = "cs"; break;
                case SystemLanguage.Danish: res = "da"; break;
                case SystemLanguage.Dutch: res = "nl"; break;
                case SystemLanguage.English: res = "en"; break;
                case SystemLanguage.Estonian: res = "et"; break;
                case SystemLanguage.Faroese: res = "fo"; break;
                case SystemLanguage.Finnish: res = "fi"; break;
                case SystemLanguage.French: res = "fr"; break;
                case SystemLanguage.German: res = "de"; break;
                case SystemLanguage.Greek: res = "el"; break;
                case SystemLanguage.Hebrew: res = "iw"; break;
                case SystemLanguage.Hungarian: res = "hu"; break;
                case SystemLanguage.Icelandic: res = "is"; break;
                case SystemLanguage.Indonesian: res = "in"; break;
                case SystemLanguage.Italian: res = "it"; break;
                case SystemLanguage.Japanese: res = "ja"; break;
                case SystemLanguage.Korean: res = "ko"; break;
                case SystemLanguage.Latvian: res = "lv"; break;
                case SystemLanguage.Lithuanian: res = "lt"; break;
                case SystemLanguage.Norwegian: res = "no"; break;
                case SystemLanguage.Polish: res = "pl"; break;
                case SystemLanguage.Portuguese: res = "pt"; break;
                case SystemLanguage.Romanian: res = "ro"; break;
                case SystemLanguage.Russian: res = "ru"; break;
                case SystemLanguage.SerboCroatian: res = "sh"; break;
                case SystemLanguage.Slovak: res = "sk"; break;
                case SystemLanguage.Slovenian: res = "sl"; break;
                case SystemLanguage.Spanish: res = "es"; break;
                case SystemLanguage.Swedish: res = "sv"; break;
                case SystemLanguage.Thai: res = "th"; break;
                case SystemLanguage.Turkish: res = "tr"; break;
                case SystemLanguage.Ukrainian: res = "uk"; break;
                case SystemLanguage.Unknown: res = "en"; break;
                case SystemLanguage.Vietnamese: res = "vi"; break;
                case SystemLanguage.ChineseSimplified: res = "zh"; break;
                case SystemLanguage.ChineseTraditional: res = "zh"; break;
            }
        
		    return res;
        }

        /// <summary>
        /// Gets RAM memory size
        /// </summary>
        /// <returns>RAM size</returns>
        internal static int GetRamSize()
        {
            return SystemInfo.systemMemorySize;  
        }

        /// <summary>
        /// Gets GPU type
        /// </summary>
        /// <returns>GPU type</returns>
        internal static string GetGpu()
        {
            return SystemInfo.graphicsDeviceName;
        }

        /// <summary>
        /// Gets device id. Using Unity API.
        /// </summary>
        /// <returns>Device ID</returns>
        internal static string GetDevideID()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }

        /// <summary>
        /// Gets diagonal size in inches
        /// </summary>
        /// <returns>Diagonal of screen</returns>
        private static float DeviceDiagonalSizeInInches()
        {
            float screenWidth = Screen.width / Screen.dpi;
            float screenHeight = Screen.height / Screen.dpi;
            float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
    
            return diagonalInches;
        }

        /// <summary>
        /// Gets device category
        /// </summary>
        /// <returns>Device category</returns>
        public static string GetDeviceCategory()
        {
            string deviceCat = "desktop";
            if(Application.isMobilePlatform)
            {
                if(Application.platform == RuntimePlatform.Android)
                {
                    float aspectRatio = Mathf.Max(Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height);
                    bool isTablet = (DeviceDiagonalSizeInInches() > 6.5f && aspectRatio < 2f);
            
                    if (isTablet)
                    {
                        deviceCat = "tablet";
                    }
                    else
                    {
                        deviceCat = "mobile";
                    }
                }
                else if(Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    bool deviceIsIpad = UnityEngine.iOS.Device.generation.ToString().Contains("iPad");
                    if (deviceIsIpad)
                    {
                        deviceCat = "tablet";
                    }
                    bool deviceIsIphone = UnityEngine.iOS.Device.generation.ToString().Contains("iPhone");
                    if (deviceIsIphone)
                    {
                        deviceCat = "mobile";
                    }
                }
            }

            return deviceCat;
        }

        /// <summary>
        /// Gets device manufacturer
        /// </summary>
        /// <returns>Device manufactuer</returns>
        public static string GetDeviceManufacturer()
        {
            string deviceModel = GetDeviceModel();
            if(string.IsNullOrEmpty(deviceModel))
            {
                return "Unknown";
            }
            
            if(deviceModel.StartsWith("iPad"))
            {
                return "Apple";
            }
            else if(deviceModel.StartsWith("iPhone"))
            {
                return "Apple";
            } else {
                string[] split = deviceModel.Split(' ');

                if(split.Length > 0)
                {
                    return split[0];
                }
                
                return "Unknown";
            }
        }

        /// <summary>
        /// Gets DPI of screen.
        /// </summary>
        /// <returns></returns>
        internal static int GetScreenColorDepth()
        {
            return (Int32)Screen.dpi;
        }

        /// <summary>
        /// Gets OS type.
        /// </summary>
        /// <returns>OS type</returns>
        public static string GetOSType() {
            return Application.platform.ToString();
        }

        /// <summary>
        /// Gets OS version
        /// </summary>
        /// <returns>OS version</returns>
        public static string GetOSVersion() {
            return SystemInfo.operatingSystem;
        }

        /// <summary>
        /// Gets Device model
        /// </summary>
        /// <returns>Device model</returns>
        public static string GetDeviceModel(){
            return SystemInfo.deviceModel;
        }

        /// <summary>
        /// Gets AppID
        /// </summary>
        /// <returns>Application ID</returns>
        public static string GetAppID() {
            return Application.identifier;
        }

        /// <summary>
        /// Gets root status
        /// </summary>
        /// <returns>Root status</returns>
        public static string GetRootStatus() {
            return Application.sandboxType.ToString();
        }

        /// <summary>
        /// Gets Device time zone in format +-hh:mm
        /// </summary>
        /// <returns>Time zone</returns>
        public static string GetDeviceTimeZone() {
            TimeSpan timezone = System.TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);

            if(timezone.Ticks < 0){
                return string.Format("-{0}" , timezone.ToString("hh':'mm"));
            } else { 
                return string.Format("+{0}" , timezone.ToString("hh':'mm"));
            }
        }

        /// <summary>
        /// Gets screen width
        /// </summary>
        /// <returns></returns>
        public static int GetScreenWidth() {
            return Screen.width;
        }

        /// <summary>
        /// Gets screen height
        /// </summary>
        /// <returns></returns>
        public static int GetScreenHeight() {
            return Screen.height;
        }

        /// <summary>
        /// Gets time since init in seconds.
        /// </summary>
        /// <returns>Time since init</returns>
        public static float GetTimeSinceStartup() {
            try
            {
                return UnityMainThreadDispatcher.Instance.GetTimeSinceInit();
            }
            catch
            {
                return 0;
            }            
        }
    }
}

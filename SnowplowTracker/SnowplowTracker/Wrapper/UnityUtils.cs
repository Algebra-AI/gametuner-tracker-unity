using System;
using SnowplowTracker.Enums;
using UnityEngine;

namespace SnowplowTracker.Wrapper
{
    /// <summary>
    /// Colect device data using Unity API.
    /// </summary>
    public class UnityUtils
    {
        /// <summary>
        /// Gets device platform
        /// </summary>
        /// <returns>Device platform</returns>
        public static DevicePlatforms GetDevicePlatform() { 
            DevicePlatforms platform = DevicePlatforms.Mobile;

            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    platform = DevicePlatforms.Mobile;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    platform = DevicePlatforms.Mobile;
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

        internal static string GetDeviceManufacturer()
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
        private static string GetRootStatus() {
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
    }
}

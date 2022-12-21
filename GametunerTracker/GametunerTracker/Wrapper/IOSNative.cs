using System;
using UnityEngine;
using UnityEngine.iOS;

namespace GametunerTracker
{
    internal static class IOSNative
    {
        public static string GetIDFA() {

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return Device.advertisingIdentifier;
            }
            return string.Empty;
        }

        public static string GetIDFV() {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return Device.vendorIdentifier;
            }
            return string.Empty;
        }
    }
}

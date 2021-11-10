using System;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace SnowplowTracker.Wrapper
{
    public static class IOSNative
    {
        public static string GetIDFA() {
#if UNITY_IOS
            return Device.advertisingIdentifier;
#else
            return string.Empty;
#endif
        }

        public static string GetIDFV() {
#if UNITY_IOS
            return Device.vendorIdentifier;
#else
            return string.Empty;
#endif
        }
    }
}

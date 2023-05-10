using System;
using UnityEngine;
using GametunerTracker.Logging;

namespace GametunerTracker
{
    internal static class AndroidNative
    {
        public static string GetAdvertisingID() {
            //TODO - napraviti da bude safe, tj sta ako nema instaliran google play service
            //Uraditi advertising id za Amazon
            string advertisingID = string.Empty;
            if (Application.platform == RuntimePlatform.Android)
            {
                try
                {
                    AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
                    AndroidJavaClass client = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient");
                    AndroidJavaObject adInfo = client.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", currentActivity);

                    advertisingID = adInfo.Call<string>("getId").ToString();
                }
                catch (System.Exception e)
                {
                    Log.Debug("Advertising ID is not collected: " + e.Message);
                }

                return advertisingID;
            }
            Log.Debug("GetAdvertisingID is not supported on this platform");
            
            return advertisingID;
        }
    }
}

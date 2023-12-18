/*
 * AndroidNative.cs
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

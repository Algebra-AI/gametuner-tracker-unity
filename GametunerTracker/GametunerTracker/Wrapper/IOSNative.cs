/*
 * IOSNative.cs
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

/*
 * GametunerEditorFix.cs
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

namespace GametunerTracker
{
    /// <summary>
    /// Script that creates GameObject and adds the GametunerEditorFix component to it.
    /// It fix the issue in Unity editor when you stop playing the game, sesson checker continues to run.
    /// </summary>
    internal class GametunerEditorFix : MonoBehaviour
    {
        private static GametunerEditorFix _instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init() {
            if (Application.isEditor)
            {
                if (_instance == null)
                {
                    _instance = new GameObject("GametunerEditorFix").AddComponent<GametunerEditorFix>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
            }
        }

        private void OnDisable() {
            GametunerUnityTracker.StopEventTracking();
        }
    }
}

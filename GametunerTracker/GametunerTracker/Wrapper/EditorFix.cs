using System;
using UnityEngine;

namespace SnowplowTracker.Wrapper
{
    /// <summary>
    /// Script that creates GameObject and adds the SnowplowEditorFix component to it.
    /// It fix the issue in Unity editor when you stop playing the game, sesson checker continues to run.
    /// </summary>
    internal class SnowplowEditorFix : MonoBehaviour
    {
        private static SnowplowEditorFix _instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init() {
            if (Application.isEditor)
            {
                if (_instance == null)
                {
                    _instance = new GameObject("SnowplowEditorFix").AddComponent<SnowplowEditorFix>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
            }
        }

        private void OnDisable() {
            ClientTracker.StopEventTracking();
        }
    }
}

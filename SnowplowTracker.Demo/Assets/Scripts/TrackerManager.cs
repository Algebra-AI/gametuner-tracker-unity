using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SnowplowTracker;
using SnowplowTracker.Wrapper;
using System;
using UnityEngine.UI;

public class TrackerManager : MonoBehaviour
{
    private void InitSnowplow()
    {
        string testURL = "34.145.73.5:8080";
        string prodURL = "woka.data.twodesperados.com:443";
        SnowplowTracker.Log.SetLogLevel(3);
        SnowplowTracker.Log.On();
        ClientTracker.Init(prodURL, "wokawoka", "Editor", "test_user", true);    
    }

    public static void LogEventSnowplow(string eventName, string schemaVersion, Dictionary<string, object> parameters, int priority = 0) {
        ClientTracker.LogEvent(eventName, schemaVersion, parameters, priority);
    }

    /// <summary>
    /// Init tracker
    /// </summary>
    private void Start() {
        InitSnowplow();
    }
}

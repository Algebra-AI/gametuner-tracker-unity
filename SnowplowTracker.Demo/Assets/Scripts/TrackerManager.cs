using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using GametunerTracker;

public class TrackerManager : MonoBehaviour
{
    //private static string _collectorUrl = "woka.data.twodesperados.com";
    private static string _collectorUrl = "localhost:9090";

    private static string _analyticsAppID = "nonocrossing";
    //private static string _analyticsAppID = "violasquest";

    private static string _apiKey = "secret-key";

    /// <summary>
    /// Adds an event listener if a Collector URL Input Field is hooked up to this MonoBehaviour
    /// </summary>
    private void Start() {
        //SnowplowTracker.Log.SetLogLevel(3);
        //SnowplowTracker.Log.On();
        //SnowplowTracker.Wrapper.ClientTracker.Init(_collectorUrl, _analyticsAppID, "Editor", true, false, "user_id");
        GametunerUnityTracker.EnableLogging();
        GametunerUnityTracker.Init(_analyticsAppID, _apiKey, false);
    }

    public static void LogEvent(string eventName, string schemaVersion, Dictionary<string, object> eventData, List<SnowplowTracker.Wrapper.ContextName> contexts = null) { 
        //SnowplowTracker.Wrapper.ClientTracker.LogEvent(eventName, schemaVersion, eventData, 0, contexts);
        GametunerUnityTracker.LogEvent(eventName, schemaVersion, eventData, 0);
    }
}

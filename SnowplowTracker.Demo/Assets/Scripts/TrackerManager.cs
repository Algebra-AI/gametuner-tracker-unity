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

    private static string _analyticsAppID = "wokawoka";
    //private static string _analyticsAppID = "violasquest";

    /// <summary>
    /// Adds an event listener if a Collector URL Input Field is hooked up to this MonoBehaviour
    /// </summary>
    private void Start() {
        //SnowplowTracker.Log.SetLogLevel(3);
        //SnowplowTracker.Log.On();
        //SnowplowTracker.Wrapper.ClientTracker.Init(_collectorUrl, _analyticsAppID, "Editor", true, false, "user_id");
        GametunerTracker.ClientTracker.EnableLogging();
        GametunerTracker.ClientTracker.Init("testgame", "api_key", false, "user_id", "Editor");
    }

    public static void LogEvent(string eventName, string schemaVersion, Dictionary<string, object> eventData, List<SnowplowTracker.Wrapper.ContextName> contexts = null) { 
        //SnowplowTracker.Wrapper.ClientTracker.LogEvent(eventName, schemaVersion, eventData, 0, contexts);
        GametunerTracker.ClientTracker.LogEvent(eventName, schemaVersion, eventData, 0);
    }
}

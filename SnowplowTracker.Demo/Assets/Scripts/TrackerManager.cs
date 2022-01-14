using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using SnowplowTracker.Wrapper;

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
        ClientTracker.Init(_collectorUrl, _analyticsAppID, "Editor", true, false, "user_id");
    }

    public static void LogEvent(string eventName, string schemaVersion, Dictionary<string, object> eventData, List<ContextName> contexts = null) { 
        ClientTracker.LogEvent(eventName, schemaVersion, eventData, 0, contexts);
    }
}

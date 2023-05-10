using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using GametunerTracker;

public class TrackerManager : MonoBehaviour
{
    private static string _analyticsAppID = "gametunerdemo";

    private static string _apiKey = "tj35xHHBhLQjykkf";

    /// <summary>
    /// Adds an event listener if a Collector URL Input Field is hooked up to this MonoBehaviour
    /// </summary>
    private void Start()
    {
        GametunerUnityTracker.EnableLogging();
        GametunerUnityTracker.Init(_analyticsAppID, _apiKey, false);
    }

    public static void LogEvent(string eventName, string schemaVersion, Dictionary<string, object> eventData, List<SnowplowTracker.Wrapper.ContextName> contexts = null)
    {
        GametunerUnityTracker.LogEvent(eventName, schemaVersion, eventData, 0);
    }

    public static void LogCurrencyChange() {
        GametunerUnityTracker.LogEventCurrencyChange("currency", 1);
    }

    public static void LogAdStarted() {
        GametunerUnityTracker.LogEventAdStarted("placement");
    }
     public static void LogAdWatched() {
        GametunerUnityTracker.LogEventAdWatched("placement", true);
    }
}

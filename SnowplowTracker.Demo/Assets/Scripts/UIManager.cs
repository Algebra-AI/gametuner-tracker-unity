using System;
using System.Collections;
using System.Collections.Generic;
using SnowplowTracker;
using SnowplowTracker.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text MessageToUpdate;

    private static string _message;

    /// <summary>
    /// Will attempt to update a Text UI element if a UI Object is linked to this Behaviour
    /// </summary>
    private void Start() {
        if (MessageToUpdate != null) MessageToUpdate.text = _message;
    }

    /// <summary>
    /// Loads the Gameplay scene
    /// </summary>
    /// <param name="restart"></param>
    public void LoadGameScene(bool restart)
    {           
        SceneManager.LoadSceneAsync("GameplayScene").completed += (x) => {
            Dictionary<string, object> eventAttribute = new Dictionary<string, object>();
            eventAttribute.Add("level", 5);
            eventAttribute.Add("group_id", "group11");
            eventAttribute.Add("is_unlimited_lives_active", false);
            eventAttribute.Add("replay", false);
            eventAttribute.Add("easy_mode", true);
            eventAttribute.Add("game_mode", "normal");
            eventAttribute.Add("level_version", "1");
            eventAttribute.Add("settings_id", "1_1");
            
            TrackerManager.LogEvent(EventNames.EVENT_LEVEL_STARTED, "1-0-0", eventAttribute);
        };
    }

    /// <summary>
    /// Loads the end game scene
    /// Creates a message to be displayed on the end game scene
    /// </summary>
    /// <param name="timeToComplete"></param>
    public void LoadEndScene(TimeSpan timeToComplete)
    {
        _message = $"Time to Complete: {timeToComplete.TotalSeconds.ToString("0.00")}s";

        SceneManager.LoadSceneAsync("EndScene").completed += (x) => {
            Dictionary<string, object> eventAttribute = new Dictionary<string, object>();
            eventAttribute.Add("level", 5);
            eventAttribute.Add("group_id", "group1");
            eventAttribute.Add("is_unlimited_lives_active", true);
            eventAttribute.Add("replay", false);
            eventAttribute.Add("easy_mode", false);
            eventAttribute.Add("game_mode", "normal");
            eventAttribute.Add("level_version", "1_30");
            eventAttribute.Add("settings_id", "1_1");
            eventAttribute.Add("time_started", "11100");
            eventAttribute.Add("time_played", 36.5f);
            eventAttribute.Add("passed", true);
            eventAttribute.Add("score", 10000);
            eventAttribute.Add("rewinds_used", 5);
            eventAttribute.Add("rewinds_used_paid", 2);
            eventAttribute.Add("gold_spent", 50);
            eventAttribute.Add("boosters_used", 3);
            eventAttribute.Add("purple_stars_collected", 1);

            TrackerManager.LogEvent(EventNames.EVENT_LEVEL_PLAYED, "1-0-0", eventAttribute);
        };
    }

    public void TriggerTestEvent()
    { 
        Dictionary<string, object> eventAttribute = new Dictionary<string, object>();
        eventAttribute.Add("group_id", "group111");
        eventAttribute.Add("registration_time", "1000");
        eventAttribute.Add("levels_passed", 10);
        eventAttribute.Add("gold", 20);
        eventAttribute.Add("lives", 2);
        eventAttribute.Add("purple_stars", 3);
        Dictionary<string, string> curr = new Dictionary<string, string>();
        curr.Add("booster1", "1");
        eventAttribute.Add("currencies", curr);
        eventAttribute.Add("unlimited_lives_active", "none");
        eventAttribute.Add("lifetime_usd_spent", 15.4f);
        eventAttribute.Add("lifetime_ads_watched", 35);
        eventAttribute.Add("trigger", "Test");
        
        TrackerManager.LogEvent(EventNames.EVENT_USER_STATE, "1-0-0", eventAttribute);
    }
}

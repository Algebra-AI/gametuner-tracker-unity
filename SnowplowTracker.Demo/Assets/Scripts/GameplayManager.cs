using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SnowplowTracker;
using SnowplowTracker.Events;
using SnowplowTracker.Payloads;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public UIManager uiManager;

    private Stopwatch _stopwatch;

    /// <summary>
    /// Adds colliders to edge of screen and begins timing for game
    /// </summary>
    void Start()
    {
        AddEdgeColliders();

        _stopwatch = new Stopwatch();
        _stopwatch.Restart();

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
    }

    /// <summary>
    /// Checks for end game condition (when no cubes are left)
    /// </summary>
    private void Update()
    {
        var cubes = GameObject.FindGameObjectsWithTag("Cube");
        if (cubes.Length == 0 && _stopwatch.IsRunning)
        {
            _stopwatch.Stop();

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

            uiManager.LoadEndScene(_stopwatch.Elapsed);
        }
    }

    /// <summary>
    /// Adds an Edge Collider across left, top and right of screen based on camera
    /// </summary>
    private void AddEdgeColliders()
    {
        var bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        var topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, Camera.main.nearClipPlane));
        var topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Camera.main.nearClipPlane));
        var bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, Camera.main.nearClipPlane));

        gameObject.AddComponent<EdgeCollider2D>().points = new Vector2[] { bottomLeft, topLeft, topRight, bottomRight };
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    
        TrackerManager.LogEventSnowplow("Start", "1-0-0", null, 0);
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
            TrackerManager.LogEventSnowplow("TimeToComplete", "1-0-0", null, 0);
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

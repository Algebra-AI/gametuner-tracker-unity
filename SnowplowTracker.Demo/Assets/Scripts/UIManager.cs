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
            //TODO: add level_started event
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
            //TODO: add level_played event
        };
    }

    public void TriggerTestEvent()
    {
        TrackerManager.LogCurrencyChange();
    }
}

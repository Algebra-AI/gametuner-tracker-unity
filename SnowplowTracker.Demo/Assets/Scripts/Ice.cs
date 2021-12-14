using System.Collections.Generic;
using SnowplowTracker;
using SnowplowTracker.Events;
using UnityEngine;

public class Ice : MonoBehaviour
{
    /// <summary>
    /// Destroys Ice Cube when colliding with Snowball
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.name == "Snowball")
            Destroy(gameObject);  

        Dictionary<string, object> eventAttribute = new Dictionary<string, object>();
        eventAttribute.Add("stash", 5);
        eventAttribute.Add("amount", 10);
        eventAttribute.Add("currency_id", "gold");
        eventAttribute.Add("group_id", "group1");
        eventAttribute.Add("reason", "gift");
        TrackerManager.LogEvent(EventNames.EVENT_CURRENCY_CHANGE, "1-0-0", eventAttribute);
    }
}

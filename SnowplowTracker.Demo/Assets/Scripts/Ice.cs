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

        TrackerManager.LogEventSnowplow("IceBlockDestroy", "1-0-0", null, 0);
    }
}

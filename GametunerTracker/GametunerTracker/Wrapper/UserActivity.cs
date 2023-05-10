using System;
using GametunerTracker.Logging;

namespace GametunerTracker
{
    public static class UserActivity
    {
        public delegate void UserActivityDelegate(long millisecondsSinceLastActivity);
        public static UserActivityDelegate OnUserActivity;

        private static long lastActivityTimestamp;

        public static void UpdateLastActivityTimestamp(long timestamp)
        {
            lastActivityTimestamp = timestamp;
        }

        public static long GetLastActivityTimestamp()
        {
            return lastActivityTimestamp;
        }

        public static void PingActivity(long timestamp)
        {            
            long timeSinceLastActivity = timestamp - lastActivityTimestamp;
            OnPingActivity(timeSinceLastActivity * 1000);
            UpdateLastActivityTimestamp(timestamp);
        }

        public static long GetTimeSinceLastActivity(long timestamp)
        {
            return timestamp - lastActivityTimestamp;
        }

        private static void OnPingActivity(long millisecondsSinceLastActivity)
        {
            if (OnUserActivity != null)
            {
                OnUserActivity(millisecondsSinceLastActivity);
            }
        }
    }
}
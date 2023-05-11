namespace GametunerTracker
{
    internal class EventData
    {
        private long eventTimestamp;
        private float eventSessionTime;

        public long EventTimestamp { get => eventTimestamp; set => eventTimestamp = value; }
        public float EventSessionTime { get => eventSessionTime; set => eventSessionTime = value; }
    }
}
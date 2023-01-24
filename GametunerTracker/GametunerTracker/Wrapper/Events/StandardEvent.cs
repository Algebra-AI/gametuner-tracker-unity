namespace GametunerTracker { 
    public class StandardEvent {

        public string EventName { get; private set; }
        public string EventSchema { get; private set; }
        
        public StandardEvent(string eventName, string eventSchema) {
            EventName = eventName;
            EventSchema = eventSchema;
        }
    }
}
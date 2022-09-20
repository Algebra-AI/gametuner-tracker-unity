using System;
using System.Collections.Generic;
using System.Linq;
using SnowplowTracker.Emitters;
using SnowplowTracker.Enums;
using SnowplowTracker.Events;
using SnowplowTracker.Payloads;
using SnowplowTracker.Payloads.Contexts;
using SnowplowTracker.Storage;

namespace SnowplowTracker.Wrapper
{
    /// <summary>
    /// Wrapper around Snowplow tracker
    /// TODO: dodati mogucnost da se ponisti konfiguracija i ponovo incijalizuje tracker. Case moze da bude kada se user loguje preko FB-a.
    /// </summary>
    public static class ClientTracker
    {
        private static Tracker tracker;
        private static DeviceContext deviceContext;
        private static bool isInitialized;
        private const string trackerNamespace = "Snowplow.Unity";
        private const string schemaTemplate = "iglu:com.twodesperados.{0}/{1}/jsonschema/{2}";
        private static string storeName;
        private static bool sandboxMode;
        private static string appID;
        private static string runtimePlatform;
        public delegate void OnSessionStarted(string sessionId, int sessionIndex, string previousSessionId);
        public static OnSessionStarted onSessionStartEvent;  

        // PUBLIC METHODS

        /// <summary>
        /// Initialize the tracker
        /// </summary>
        /// <param name="endpointUrl">Collector server URL in form {name}.twodesperados.com:{port}</param>
        /// <param name="analyticsAppID">Analytics app id, you will get it from data team.</param>
        /// <param name="store">Store name. Eg. GooglePlay, ITunes, Amazon...</param>
        /// <param name="userID">Unique user id</param>
        public static void Init(string endpointUrl, string analyticsAppID, string store, bool isSandboxEnabled, bool useHttps = true, string userID = null, string apiKey = "") { 
            
            if (isInitialized) {
                Log.Debug("Tracker is already initialized");
                return;
            }

            try
            {         
                runtimePlatform = UnityUtils.GetRuntimePlatform();                
                appID = analyticsAppID;
                storeName = store;
                sandboxMode = isSandboxEnabled;

                UnityMainThreadDispatcher.Instance.Init();
                SnowplowEditorFix.Init();

                // Create Emitter and Tracker
                ExtendedEventStore extendedStore = new ExtendedEventStore();
                HttpProtocol protocol = useHttps ? HttpProtocol.HTTPS : HttpProtocol.HTTP;
                IEmitter emitter = new AsyncEmitter(endpointUrl, protocol, HttpMethod.POST, sendLimit: 100, 52000, 52000, extendedStore);
                
                //TODO: zameniti sekunde sa dogovorenim vrednostima
                Session session = new Session("snowplow_session_data.dict", 72000, 300, 15);
                //Session session = new Session("sessionPath", 30, 10, 2);
                session.onSessionStart += OnSessionStartEvent;
                session.onSessionEnd += OnSessionEndEvent;

                Subject subject = new Subject();
                string tempUserID = GetUserIDFromCache(extendedStore);
                if (userID != null)
                {
                    tempUserID = userID;
                    UpdateUserIDInCache(userID, extendedStore);
                }

                string installationId = GetInstallationIDFromCache(extendedStore);
                if (string.IsNullOrEmpty(installationId))
                {
                    installationId = Utils.GetGUID();
                    UpdateInstallationIDInCache(installationId, extendedStore);
                }

                subject.SetUserId(tempUserID);
                subject.SetInstallationId(installationId);
                subject.SetApiKey(apiKey);

                deviceContext = GetDeviceContext();

                tracker = new Tracker(
                            emitter, 
                            trackerNamespace, 
                            UnityUtils.GetAppID(), 
                            subject, 
                            session, 
                            UnityUtils.GetDevicePlatform(), 
                            true);

                tracker.StartEventTracking();
                
                isInitialized = true;

                if(session.GetSessionIndex() == 1) {
                    TriggerRegistration();
                    SaveRegistrationTime(extendedStore);
                }
                
                OnSessionStartEvent();
                UnityEngine.Application.focusChanged += SetFocus;
                UnityMainThreadDispatcher.Instance.onQuit += OnSessionEndOnQuit;
                Log.Debug("Tracker initialized");           
            }
            catch (System.Exception e)
            {
                Log.Error("Tracker: Not initialized: " + e.Message);
                isInitialized = false;
            } 
        }        

        /// <summary>
        /// Setup user ID
        /// </summary>
        /// <param name="userId">User ID</param>
        public static void SetUserId(string userId) {
            if (!isInitialized) { 
                Log.Debug("Tracker is not initialized");
                return;
            }

            UpdateUserIDInCache(userId, (ExtendedEventStore)(tracker.GetEmitter().GetEventStore()));
            tracker.GetSubject().SetUserId(userId);
        }

        /// <summary>
        /// Gets user ID. IF user ID is not set, returns null
        /// </summary>
        /// <returns>User ID</returns>
        public static string GetUserID() { 
            if (!isInitialized) { 
                Log.Debug("Tracker is not initialized");
                return null;
            }

            return tracker.GetSubject().GetUserID();
        }

        /// <summary>
        /// Log analytics event.
        /// </summary>
        /// <param name="eventName">Name of event</param>
        /// <param name="schemaVersion">Varsion of event schema</param>
        /// <param name="parameters">Event parameters</param>
        /// <param name="priority">Priority of event. 0 is default. Bigger the number is, priority is higher</param>
        /// <param name="contexts">List of contexts names. null is default.</param>
        public static void LogEvent(
                string eventName, 
                string schemaVersion, 
                Dictionary<string, object> parameters, 
                int priority = 0,
                List<ContextName> contexts = null) { 
            if (!isInitialized) {
                Log.Error("Tracker isn't initialized");
                return;
            }

            LogEvent(eventName, schemaVersion, parameters, GetContexts(contexts), priority); 
        }

        /// <summary>
        /// Gets registration time from cache.
        /// </summary>
        /// <returns>Unix timestamp of registration time</returns>
        public static long GetRegistrationTime() { 
            if (!isInitialized) {
                Log.Error("Tracker isn't initialized");
                return 0;
            }

            return ((ExtendedEventStore)tracker.GetEmitter().GetEventStore()).GetRegistrationTime();
        }

        /// <summary>
        /// Stops tracker.
        /// </summary>
        internal static void StopEventTracking() {
            if (!isInitialized) { 
                Log.Error("Tracker isn't initialized");
                return;
            }
            tracker.StopEventTracking();
        }

        internal static void StartEventTracking() {
            if (!isInitialized) { 
                Log.Error("Tracker isn't initialized");
                return;
            }
            tracker.StartEventTracking();
        }

        ///  PRIVATE METHODS

        /// <summary>
        /// Subscribe to session start event for Unity thread.
        /// </summary>
        private static void OnSessionStartUnityThread(string sessionId, int sessionIndex, string previousSessionId) {
            if (onSessionStartEvent != null) { 
                UnityMainThreadDispatcher.Instance.Enqueue(() => onSessionStartEvent.Invoke(sessionId, sessionIndex, previousSessionId));
            }
        }

        /// <summary>
        /// Create IContext list of ContextName
        /// </summary>
        /// <param name="contextNames">List of context names</param>
        /// <returns>List of Icontext</returns>
        private static List<IContext> GetContexts(List<ContextName> contextNames) {

            List<IContext> contexts = new List<IContext>();
            if(contextNames != null) {
                foreach(ContextName cn in contextNames) {
                    switch (cn)
                    {
                        case ContextName.DEVICE_CONTEXT:
                            contexts.Add(GetDeviceContext());
                            break;
                        default:
                            break;
                    }
                }
            }

            return contexts;
        }
    
        /// <summary>
        /// Method subscribed to sesson start event.
        /// </summary>
        private static void OnSessionStartEvent() { 
            if (!isInitialized) {
                Log.Error("Tracker isn't initialized");
                return;
            }

            Dictionary<string, object> eventParams = new Dictionary<string, object>();
            eventParams.Add("store", storeName);   
            eventParams.Add("previous_session_id", tracker.GetSession().GetPreviousSession()); 
            eventParams.Add("device_platform", runtimePlatform);
            LogEvent(EventNames.EVENT_LOGIN, "1-0-1", eventParams, new List<IContext> { deviceContext }, 1000);

            OnSessionStartUnityThread(tracker.GetSession().GetSessionID(), tracker.GetSession().GetSessionIndex(), tracker.GetSession().GetPreviousSession());
        }

        /// <summary>
        /// Called when application is closed.
        /// </summary>
        private static void OnSessionEndOnQuit()
        {
            if (!isInitialized)
            {
                Log.Error("Tracker isn't initialized");
                return;
            }
           
            OnSessionEndEvent(false);
            StopEventTracking();
        }

        /// <summary>
        /// Method subscribed to sesson end event.
        /// </summary>
        private static void OnSessionEndEvent(EventData eventData) { 
            if (!isInitialized)
            {
                Log.Error("Tracker isn't initialized");
                return;
            }
            OnSessionEndEvent(true, eventData.EventTimestamp, eventData.EventSessionTime);
        }

        /// <summary>       
        /// Call on session end.
        /// </summary>
        /// <param name="isTimeout">Is session timeouted</param>
        private static void OnSessionEndEvent(bool isTimeout, long eventTimestamp = 0, float sessionTime = 0) { 
            if (!isInitialized) {
                Log.Error("Tracker isn't initialized");
                return;
            }

            Dictionary<string, object> eventParams = new Dictionary<string, object>();
            eventParams.Add("last_event_time", tracker.GetLastTrackEventTime());   
            eventParams.Add("timeout", isTimeout); 
            LogEvent(EventNames.EVENT_LOGOUT, "1-0-0", eventParams, GetContexts(null), 100, eventTime: eventTimestamp, sessionTime);
        }

        /// <summary>
        /// Log registration event
        /// </summary>
        private static void TriggerRegistration()
        {
            if (!isInitialized) {
                Log.Error("Tracker isn't initialized");
                return;
            }

            Dictionary<string, object> eventParams = new Dictionary<string, object>();
            eventParams.Add("store", storeName);
            eventParams.Add("device_platform", runtimePlatform);  
            LogEvent(EventNames.EVENT_REGISTRATION, "1-0-1", eventParams, new List<IContext> { deviceContext }, 100);
        }    

        /// <summary>
        /// Save registration time
        /// </summary>
        /// <param name="extendedStore">Store object</param>
        private static void SaveRegistrationTime(ExtendedEventStore extendedStore)
        {
            extendedStore.SetRegistrationTime(Utils.GetTimestamp());
        }    

        /// <summary>
        /// Gets name of last triggered event
        /// </summary>
        /// <param name="currentEventName">Event that needs to be triggered</param>
        /// <returns>Name of event</returns>
        private static string GetLastEventName(string currentEventName) { 
            ExtendedEventStore store = (ExtendedEventStore)(tracker.GetEmitter().GetEventStore());
            string lastEventName = store.GetLastAddedEvent();
            store.UpdateLastAddedEvent(currentEventName);
            return lastEventName;
        }

        /// <summary>
        /// Gets event index
        /// </summary>
        /// <returns>Event index</returns>
        private static int GetEventIndex() { 
            ExtendedEventStore store = (ExtendedEventStore)(tracker.GetEmitter().GetEventStore());
            int eventIndex = store.GetEventIndex();
            store.UpdateEventIndex();
            return eventIndex;
        }

        /// <summary>
        /// Gets userID from cache.
        /// </summary>
        /// <returns>UserID</returns>
        private static string GetUserIDFromCache(ExtendedEventStore store) { 
            return store.GetCacheUserId();
        }

        /// <summary>
        /// Gets Installation id from cache.
        /// </summary>
        /// <param name="store">Event store</param>
        /// <returns>Installation ID</returns>
        private static string GetInstallationIDFromCache(ExtendedEventStore store) { 
            return store.GetInstallationId();
        }

        /// <summary>
        /// Updates userID in cache.
        /// </summary>
        /// <param name="userID"></param>
        private static void UpdateUserIDInCache(string userID, ExtendedEventStore store) { 
            store.UpdateUserId(userID);
        }

        /// <summary>
        /// Updates installation in cache.
        /// </summary>
        /// <param name="installationId">Installation ID</param>
        /// <param name="store">Event store</param>
        private static void UpdateInstallationIDInCache(string installationId, ExtendedEventStore store) { 
            store.UpdateInstallationId(installationId);
        }

        

        /// <summary>
        /// Log analytics event.
        /// </summary>
        /// <param name="eventName">Name of event</param>
        /// <param name="schema">Shema of event</param>
        /// <param name="parameters">Event parameters</param>
        /// <param name="priority">Priority of event. 0 is default. Bigger the number is, priority is higher</param>
        private static void LogEvent(
                string eventName, 
                string schemaVersion, 
                Dictionary<string, object> parameters, 
                List<IContext> context, 
                int priority,
                long eventTime = 0,
                float sessionTime = 0) { 
            if (!isInitialized) {
                Log.Error("Tracker isn't initialized");
                return;
            }
            
            
            // Create your event data
            string schema = string.Format(schemaTemplate, appID, eventName, schemaVersion);
            System.Object obj = null;
            SelfDescribingJson eventData = new SelfDescribingJson(schema, obj);
              
            if (parameters != null)
            {
                Dictionary<string, object> eventParams = new Dictionary<string, object>();               
            
                foreach (KeyValuePair<string, object> item in parameters)
                {
                    object val = item.Value;
                    if (val == null) {
                        continue;
                    }

                    if (val.GetType() == typeof(Dictionary<string, object>)) { 
                        List<Dictionary<string, object>> data_temp = new List<Dictionary<string, object>>();
                        Dictionary<string, object> dataDict = (Dictionary<string, object>)val;
                        foreach (var dictItem in dataDict)
                        {
                            data_temp.Add(new Dictionary<string, object>() { { "key", dictItem.Key }, { "value", dictItem.Value } });
                        }
                        eventParams.Add(item.Key, data_temp);
                    } else {
                        eventParams.Add(item.Key, val);
                    }
                }

                eventData = new SelfDescribingJson(schema, eventParams);
            }

            List<IContext> contextList = new List<IContext>();
            tracker.GetSession().SetLastActivityTick(UnityUtils.GetTimeSinceStartup());

            float eventSessionTime = sessionTime == 0 ? tracker.GetSession().GetSessionTime() : sessionTime;
            contextList.Add(GetEventContext(tracker.GetSession(), GetLastEventName(eventName), GetEventIndex(), eventSessionTime));
            
            if (context != null) { 
                foreach (IContext item in context)
                {
                    if(item != null) { 
                        contextList.Add(item);
                    }
                }
            }

            long eventTimestamp = eventTime == 0 ? Utils.GetTimestamp() : eventTime;
              // Track your event with your custom event data
            Unstructured  newEvent = 
                new Unstructured()
                .SetEventData(eventData)
                .SetCustomContext(contextList)
                .SetTimestamp(eventTimestamp)
                .SetEventId(Utils.GetGUID())
                .SetEventPriority(priority)
                .Build();           

            tracker.Track(newEvent);            
        }

        /// <summary>
        /// Method subscribed to focus change event.
        /// </summary>
        /// <param name="focus">Is application in focus</param>
        private static void SetFocus(bool focus) { 
            if (!isInitialized) { 
                Log.Error("Tracker isn't initialized");
                return;
            }
            
            if(focus) {
                StartEventTracking();
            } else {  
                StopEventTracking();
            }

            tracker.GetSession().SetBackground(!focus);            
        }

        /// <summary>
        /// Generate device context data. It's NOT thread safe, it should be called from main thread.
        /// </summary>
        /// <returns>Device context data</returns>
        private static DeviceContext GetDeviceContext() {
            return new DeviceContext()
                .SetAdvertisingID(AndroidNative.GetAdvertisingID())
                .SetBuildVersion(UnityUtils.GetBuildVersion())
                .SetCampaign(string.Empty)
                .SetCpuType(UnityUtils.GetCpuType())
                .SetDeviceCategory(UnityUtils.GetDeviceCategory())
                .SetDeviceId(UnityUtils.GetDevideID())
                .SetDeviceLanguage(UnityUtils.GetDeviceLanguage())
                .SetDeviceManufacturer(UnityUtils.GetDeviceManufacturer())
                .SetDeviceModel(UnityUtils.GetDeviceModel())
                .SetDeviceTimezone(UnityUtils.GetDeviceTimeZone())
                .SetGpu(UnityUtils.GetGpu())
                .SetIDFA(IOSNative.GetIDFA())
                .SetIDFV(IOSNative.GetIDFV())
                .SetIsHacked(UnityUtils.GetRootStatus())
                .SetMedium(string.Empty)
                .SetOsVersion(UnityUtils.GetOSVersion())
                .SetRamSize(UnityUtils.GetRamSize())
                .SetScreenResolution(UnityUtils.GetScreenWidth(), UnityUtils.GetScreenHeight())
                .SetSource(string.Empty)
                .Build();
        }

        /// <summary>
        /// Generate event context data. It's thread safe, it don't need to be called from main thread.
        /// </summary>
        /// <param name="sessionData">Session object data</param>
        /// <param name="lastEventName">Name of last triggered event</param>
        /// <param name="eventIndex">Index of event</param>
        /// <returns>Event context data</returns>
        private static EventContext GetEventContext(Session sessionData, string lastEventName, int eventIndex, float sessionTime) {

            float eventSessionTime = 0;
            if(sessionTime == 0) { 
                eventSessionTime = sessionData.GetSessionTime();
            }

            return new EventContext ()
                .SetEventIndex(eventIndex)
                .SetEventSessionIndex(sessionData.GetSessionIndex())
                .SetPreviousEventName(lastEventName)
                .SetSendboxMode(sandboxMode)
                .SetSessionID(sessionData.GetSessionID())
                .SetSessionTimePassed(eventSessionTime)
                .Build ();
        }
    }
}

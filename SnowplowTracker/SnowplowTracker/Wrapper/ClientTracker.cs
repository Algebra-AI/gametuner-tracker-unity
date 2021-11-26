using System;
using System.Collections.Generic;
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

        // PUBLIC METHODS

        /// <summary>
        /// Initialize the tracker
        /// </summary>
        /// <param name="endpointUrl">Collector server URL in form {name}.twodesperados.com:{port}</param>
        /// <param name="analyticsAppID">Analytics app id, you will get it from data team.</param>
        /// <param name="store">Store name. Eg. GooglePlay, ITunes, Amazon...</param>
        /// <param name="userID">Unique user id</param>
        public static void Init(string endpointUrl, string analyticsAppID, string store, string userID, bool isSandboxEnabled) { 
            
            if (isInitialized) {
                Log.Debug("Tracker is already initialized");
                return;
            }

            appID = analyticsAppID;
            storeName = store;
            sandboxMode = isSandboxEnabled;

            UnityMainThreadDispatcher.Instance.Init();
            SnowplowEditorFix.Init();

            // Create Emitter and Tracker
            ExtendedEventStore extendedStore = new ExtendedEventStore();
            IEmitter emitter;
            Session session = new Session("snowplow_session_data.dict", 72000, 1800, 15);
            if (UnityUtils.IsWebGLPlatform())
            {
                emitter = new SyncEmitter(endpointUrl, HttpProtocol.HTTPS, HttpMethod.POST, sendLimit: 2, 52000, 52000, extendedStore);
                session = new SyncSession("snowplow_session_data.dict", 72000, 1800, 15);
            } else { 
                emitter = new AsyncEmitter(endpointUrl, HttpProtocol.HTTPS, HttpMethod.POST, sendLimit: 100, 52000, 52000, extendedStore);
                session = new Session("snowplow_session_data.dict", 72000, 1800, 15);
            }

            //TODO: zameniti sekunde sa dogovorenim vrednostima
            //Session session = new Session("snowplow_session_data.dict", 72000, 1800, 15);
            //Session session = new Session("sessionPath", 120, 30, 15);
            session.onSessionStart = OnSessionStartEvent;
            session.onSessionEnd = OnSessionEndEvent;

            Subject subject = new Subject();
            subject.SetUserId(userID);

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
            }
            
            OnSessionStartEvent();
            UnityEngine.Application.focusChanged += SetFocus;
            UnityMainThreadDispatcher.Instance.onQuit += OnSessionEndOnQuit;
            Log.Debug("Tracker initialized");            
        }

        /// <summary>
        /// Log analytics event.
        /// </summary>
        /// <param name="eventName">Name of event</param>
        /// <param name="schemaVersion">Varsion of event schema</param>
        /// <param name="parameters">Event parameters</param>
        /// <param name="priority">Priority of event. 0 is default. Bigger the number is, priority is higher</param>
        public static void LogEvent(string eventName, string schemaVersion, Dictionary<string, object> parameters, int priority = 0) { 
            if (!isInitialized) {
                Log.Error("Tracker isn't initialized");
                return;
            }

            LogEvent(eventName, schemaVersion, parameters, null, priority); 
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

        ///  PRIVATE METHODS
    
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
            LogEvent(EventNames.EVENT_LOGIN, "1-0-0", eventParams, new List<IContext> { deviceContext });
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
        }

        /// <summary>
        /// Method subscribed to sesson end event.
        /// </summary>
        private static void OnSessionEndEvent() { 
            if (!isInitialized)
            {
                Log.Error("Tracker isn't initialized");
                return;
            }
            OnSessionEndEvent(true);
        }

        /// <summary>       
        /// Call on session end.
        /// </summary>
        /// <param name="isTimeout">Is session timeouted</param>
        private static void OnSessionEndEvent(bool isTimeout) { 
            if (!isInitialized) {
                Log.Error("Tracker isn't initialized");
                return;
            }

            Dictionary<string, object> eventParams = new Dictionary<string, object>();
            eventParams.Add("last_event_time", tracker.GetLastTrackEventTime());   
            eventParams.Add("timeout", isTimeout); 
            LogEvent(EventNames.EVENT_LOGOUT, "1-0-0", eventParams);
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
            LogEvent(EventNames.EVENT_REGISTRATION, "1-0-0", eventParams, new List<IContext> { deviceContext } );
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


        private static int GetEventIndex() { 
            ExtendedEventStore store = (ExtendedEventStore)(tracker.GetEmitter().GetEventStore());
            int eventIndex = store.GetEventIndex();
            store.UpdateEventIndex();
            return eventIndex;
        }

        /// <summary>
        /// Log analytics event.
        /// </summary>
        /// <param name="eventName">Name of event</param>
        /// <param name="schema">Shema of event</param>
        /// <param name="parameters">Event parameters</param>
        /// <param name="priority">Priority of event. 0 is default. Bigger the number is, priority is higher</param>
        private static void LogEvent(string eventName, string schemaVersion, Dictionary<string, object> parameters, List<IContext> context, int priority = 0) { 
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
                        val = string.Empty;
                    }

                    if (val.GetType() == typeof(Dictionary<string, object>)) { 
                        List<Dictionary<string, object>> data_temp = new List<Dictionary<string, object>>();
                        Dictionary<string, object> dataDict = (Dictionary<string, object>)val;
                        foreach (var dictItem in dataDict)
                        {
                            data_temp.Add(new Dictionary<string, object>() { { "key", item.Key }, { "value", val } });
                        }
                        eventParams.Add(item.Key, data_temp);
                    } else {
                        eventParams.Add(item.Key, val);
                    }
                }

                eventData = new SelfDescribingJson(schema, eventParams);
            }

            List<IContext> contextList = new List<IContext>();              
            contextList.Add(GetEventContext(tracker.GetSession(), GetLastEventName(eventName), GetEventIndex()));
            
            if (context != null) { 
                foreach (IContext item in context)
                {
                    if(item != null) { 
                        contextList.Add(item);
                    }
                }
            }
              // Track your event with your custom event data
            Unstructured  newEvent = 
                new Unstructured()
                .SetEventData(eventData)
                .SetCustomContext(contextList)
                .SetTimestamp(Utils.GetTimestamp())
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
        private static EventContext GetEventContext(Session sessionData, string lastEventName, int eventIndex) {

            return new EventContext ()
                .SetEventIndex(eventIndex)
                .SetEventSessionIndex(sessionData.GetSessionIndex())
                .SetPreviousEventName(lastEventName)
                .SetSendboxMode(sandboxMode)
                .SetSessionID(sessionData.GetSessionID())
                .Build ();
        }
    }
}

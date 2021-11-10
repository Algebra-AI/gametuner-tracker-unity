using System;
using System.Collections.Generic;
using SnowplowTracker.Emitters;
using SnowplowTracker.Enums;
using SnowplowTracker.Events;
using SnowplowTracker.Payloads;
using SnowplowTracker.Payloads.Contexts;
using SnowplowTracker.Storage;
using UnityEngine;

namespace SnowplowTracker.Wrapper
{
    /// <summary>
    /// Wrapper around Snowplow tracker
    /// TODO: dodati mogucnost da se ponisti konfiguracija i ponovo incijalizuje tracker. Case moze da bude kada se user loguje preko FB-a.
    /// </summary>
    public static class ClientTracker
    {
        private static Tracker tracker;
        private static bool isInitialized;
        private const string endPoint = "34.145.73.5:8080";
        private const string trackerNamespace = "Snowplow.Unity";
        
        /// <summary>
        /// Initialize the tracker
        /// </summary>
        /// <param name="userID">Unique user ID</param>
        public static void Init(string userID) { 
            
            if (isInitialized) {
                Log.Debug("Tracker is already initialized");
                return;
            }

            UnityMainThreadDispatcher.Instance.Init();
#if UNITY_EDITOR
            SnowplowEditorFix.Init();
#endif

            // Create Emitter and Tracker
            ExtendedEventStore extendedStore = new ExtendedEventStore();
            IEmitter emitter = new AsyncEmitter(endPoint, HttpProtocol.HTTP, HttpMethod.POST, sendLimit: 100, 52000, 52000, extendedStore);
            
            //TODO: zameniti sekunde sa dogovorenim vrednostima
            Session session = new Session("sessionPath", 72000, 300, 15);
            //Session session = new Session("sessionPath", 120, 30, 15);
            session.onSessionStart = OnSessionStartEventSafe;
            session.onSessionEnd = OnSessionEndEventSafe;

            Subject subject = new Subject();
            subject.SetUserId(userID);
            subject.SetLanguage(UnityUtils.GetDeviceLanguage());
            subject.SetTimezone(UnityUtils.GetDeviceTimeZone());
            subject.SetScreenResolution(UnityUtils.GetScreenWidth(), UnityUtils.GetScreenHeight());
            subject.SetColorDepth(UnityUtils.GetScreenColorDepth());

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
            
            OnSessionStartEvent();
            Application.focusChanged += SetFocus;
            UnityMainThreadDispatcher.Instance.onQuit = OnSessionEndEvent;
            Log.Debug("Tracker initialized");            
        }

        private static MobileContext GetContextMobile() {

            return new MobileContext ()
                .SetOsType(UnityUtils.GetOSType())
                .SetOsVersion(UnityUtils.GetOSVersion())
                .SetDeviceManufacturer(UnityUtils.GetDeviceManufacturer())
                .SetDeviceModel(UnityUtils.GetDeviceModel())
                .SetCarrier("none")
                .SetNetworkType(UnityUtils.GetNetworkType())
                .SetNetworkTechnology("none")
                .SetAppleIdfa(IOSNative.GetIDFA())
                .SetAppleIdfv(IOSNative.GetIDFV())
                .SetAndroidIdfa(AndroidNative.GetAdvertisingID())
                .Build ();
        }

        /// <summary>
        /// Method subscribed to sesson start event.
        /// </summary>
        private static void OnSessionStartEventSafe()
        {
            if (!isInitialized) {
                Log.Error("Tracker isn't initialized");
                return;
            }
            
            UnityMainThreadDispatcher.Instance.Enqueue(() => OnSessionStartEvent());         
        }

        private static void OnSessionStartEvent() { 
            if (!isInitialized) {
                Log.Error("Tracker isn't initialized");
                return;
            }

            LogEvent("login", "iglu:com.twodesperados/event_testing/jsonschema/3-0-0", null);
        }

        /// <summary>
        /// Method subscribed to sesson end event.
        /// </summary>
        private static void OnSessionEndEventSafe()
        {
            if (!isInitialized) {
                Log.Error("Tracker isn't initialized");
                return;
            }
            
            UnityMainThreadDispatcher.Instance.Enqueue(() => OnSessionEndEvent());         
        }

        private static void OnSessionEndEvent() { 
            if (!isInitialized) {
                Log.Error("Tracker isn't initialized");
                return;
            }

            LogEvent("logout", "iglu:com.twodesperados/event_testing/jsonschema/3-0-0", null);
        }

        /// <summary>
        /// Log analytics event.
        /// </summary>
        /// <param name="eventName">Name of event</param>
        /// <param name="schema">Shema of event</param>
        /// <param name="parameters">Event parameters</param>
        /// <param name="priority">Priority of event. 0 is default. Bigger the number is, priority is higher</param>
        public static void LogEvent(string eventName, string schema, Dictionary<string, object> parameters, int priority = 0) { 
            if (!isInitialized) {
                Log.Error("Tracker isn't initialized");
                return;
            }
            // Create your event data
            //iglu:com.twodesperados/event_testing/jsonschema/3-0-0
            List<Dictionary<string, string>> data_temp = new List<Dictionary<string, string>>();
            if (parameters != null)
            {                
                foreach (var item in parameters)
                {
                    data_temp.Add(new Dictionary<string, string>() { { "key", item.Key }, { "value", item.Value.ToString() } });
                }
            }

            data_temp.Add(new Dictionary<string, string>() {  { "key", "eventName" }, { "value", eventName } });

            ExtendedEventStore store = (ExtendedEventStore)(tracker.GetEmitter().GetEventStore());
            data_temp.Add(new Dictionary<string, string>() {  { "key", "previousEvent" }, { "value", store.GetLastAddedEvent() } });
            store.UpdateLastTriggeredEvent(eventName);

            SelfDescribingJson eventData = new SelfDescribingJson(schema, data_temp);
    
            List<IContext> contextList = new List<IContext>();
            contextList.Add(GetContextMobile());

            // Track your event with your custom event data
            Unstructured newEvent = 
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
        /// Stops tracker.
        /// </summary>
        public static void StopEventTracking() {
            if (!isInitialized) { 
                Log.Error("Tracker isn't initialized");
                return;
            }
            tracker.StopEventTracking();
        }

        /// <summary>
        /// Method subscribed to focus change event.
        /// </summary>
        /// <param name="focus">Is application in focus</param>
        public static void SetFocus(bool focus) { 
            if (!isInitialized) { 
                Log.Error("Tracker isn't initialized");
                return;
            }
            
            tracker.GetSession().SetBackground(!focus);
        }
    }
}

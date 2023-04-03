using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace GametunerTracker
{
    /// <summary>
    /// A thread-safe class which holds a queue with actions to execute on the next Update() method. It can be used to make calls to the main thread for
    /// things such as UI Manipulation in Unity. It was developed for use in combination with the Snowplow Unity plugin, which uses separate threads for event handling
    /// </summary>
    internal class UnityMainThreadDispatcher : MonoBehaviour
    {

        private static readonly Queue<Action> _executionQueue = new Queue<Action>();
        private static volatile float timeSinceInit = 0f;
        private static bool dispatcherInitialized = false;
        public delegate void OnFocus(bool focus);
        public delegate void OnQuit();
        public OnFocus onFocus;
        public OnQuit onQuit;


        public void FixedUpdate()
        {
            lock (_executionQueue)
            {
                while (_executionQueue.Count > 0)
                {
                    _executionQueue.Dequeue().Invoke();
                }
            }

            if (dispatcherInitialized) {
                timeSinceInit = Time.realtimeSinceStartup;
            }
        }

        /// <summary>
        /// Gets the time since the dispatcher was initialized
        /// </summary>
        /// <returns>Time since Init</returns>
        public float GetTimeSinceInit() {
            return timeSinceInit;
        }

        /// <summary>
        /// Locks the queue and adds the IEnumerator to the queue
        /// </summary>
        /// <param name="action">IEnumerator function that will be executed from the main thread.</param>
        public void Enqueue(IEnumerator action)
        {
            lock (_executionQueue)
            {
                _executionQueue.Enqueue(() =>
                {
                    StartCoroutine(action);
                });
            }
        }

        /// <summary>
        /// Locks the queue and adds the Action to the queue
        /// </summary>
        /// <param name="action">function that will be executed from the main thread.</param>
        public void Enqueue(Action action)
        {
            Enqueue(ActionWrapper(action));
        }

        /// <summary>
        /// Locks the queue and adds the Action to the queue, returning a Task which is completed when the action completes
        /// </summary>
        /// <param name="action">function that will be executed from the main thread.</param>
        /// <returns>A Task that can be awaited until the action completes</returns>
        public Task EnqueueAsync(Action action)
        {
            var tcs = new TaskCompletionSource<bool>();

            void WrappedAction()
            {
                try
                {
                    action();
                    tcs.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }

            Enqueue(ActionWrapper(WrappedAction));
            return tcs.Task;
        }


        IEnumerator ActionWrapper(Action a)
        {
            a();
            yield return null;
        }

        public void Init()
        {
            dispatcherInitialized = true;
        }

        /// <summary>
        /// When the application loses focus, the dispatcher will stop updating.
        /// </summary>
        /// <param name="focus">Focus</param>
        /// <returns></returns>
        private  IEnumerator OnApplicationFocus(bool focus)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForFixedUpdate();

            if (onFocus != null)
            {
                onFocus(focus);
            }
        }

        #region SINGLETONE    
        private static UnityMainThreadDispatcher _instance;
        private static object _lock = new object();
        protected UnityMainThreadDispatcher() { }
        public static UnityMainThreadDispatcher Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    return null;
                }
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (UnityMainThreadDispatcher)FindObjectOfType(typeof(UnityMainThreadDispatcher));
                        if (FindObjectsOfType(typeof(UnityMainThreadDispatcher)).Length > 1)
                        {
                            return _instance;
                        }
                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<UnityMainThreadDispatcher>();
                            singleton.name = "(singleton) " + typeof(UnityMainThreadDispatcher).ToString();
                            DontDestroyOnLoad(singleton);
                        }
                        else
                        {
                        }
                    }
                    return _instance;
                }
            }
        }



        private static bool applicationIsQuitting = false;
        /// <summary>     
        /// /// When Unity quits, it destroys objects in a random order.     
        /// /// In principle, a Singleton is only destroyed when application quits.    
        /// /// If any script calls Instance after it have been destroyed,     
        /// ///   it will create a buggy ghost object that will stay on the Editor scene   
        /// ///   even after stopping playing the Application. Really bad!   
        /// /// So, this was made to be sure we're not creating that buggy ghost object. 
        /// /// </summary>    
        public void OnDestroy()
        {
            applicationIsQuitting = true;
        }

        void OnApplicationQuit()
        {
            if (onQuit != null)
            {
                onQuit();
            }
        }
        #endregion
    }
}
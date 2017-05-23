using System;
using System.Collections.Generic;

namespace ThreeK.Game.Event
{

    public class FEvent
    {
        public object Data { get; set; }

        public FEvent() : this(null)
        {
        }

        public FEvent(object data)
        {
            Data = data;
        }
    }

    /// <summary>
    /// Use case:
    /// dispatcher.AddListener<MyEvent>(OnEventHappened);
    /// dispatcher.RemoveListener<MyEvent>(OnEventHappened);
    /// void OnEventHappened (MyEvent event) {};
    /// dispatcher.Dispatch(new MyEvent());
    /// dispatcher.DispatchWith<MyEvent>();
    /// </summary>
    public class EventDispatcher
    {
        private static readonly Dictionary<Type, FEvent> EventCache = new Dictionary<Type, FEvent>();

        public delegate void EventDelegate<T>(T e) where T : FEvent;

        private delegate void EventDelegate(FEvent e);

        private readonly Dictionary<Type, EventDelegate> _delegates = new Dictionary<Type, EventDelegate>();
        private readonly Dictionary<Delegate, EventDelegate> _delegateLookup = new Dictionary<Delegate, EventDelegate>();

        private string _uid;
        public string UID
        {
            get { return _uid; }
        }

        public EventDispatcher()
        {
            _uid = new Random().Next().ToString();
        }

        public void AddListener<T>(EventDelegate<T> del) where T : FEvent
        {
            // Early-out if we've already registered this delegate
            if (_delegateLookup.ContainsKey(del))
                return;

            // Create a new non-generic delegate which calls our generic one.
            // This is the delegate we actually invoke.
            EventDelegate internalDelegate = (e) => del((T)e);
            _delegateLookup[del] = internalDelegate;

            EventDelegate tempDel;
            if (_delegates.TryGetValue(typeof(T), out tempDel))
            {
                _delegates[typeof(T)] = tempDel += internalDelegate;
            }
            else
            {
                _delegates[typeof(T)] = internalDelegate;
            }
        }

        public void RemoveListener<T>(EventDelegate<T> del) where T : FEvent
        {
            EventDelegate internalDelegate;
            if (_delegateLookup.TryGetValue(del, out internalDelegate))
            {
                EventDelegate tempDel;
                if (_delegates.TryGetValue(typeof(T), out tempDel))
                {
                    tempDel -= internalDelegate;
                    if (tempDel == null)
                        _delegates.Remove(typeof(T));
                    else
                        _delegates[typeof(T)] = tempDel;
                }

                _delegateLookup.Remove(del);
            }
        }

        /*public void RemoveListeners<T>() where T : FEvent
        {
            EventDelegate tempDel;
            if (_delegates.TryGetValue(typeof(T), out tempDel))
            {
                foreach (Delegate del in tempDel.GetInvocationList())
                {
                    var toRemove = new List<Delegate>();
                    foreach (KeyValuePair<Delegate, EventDelegate> pair in _delegateLookup)
                    {
                        if (pair.Value == del) toRemove.Add(pair.Key);
                    }
                    foreach (Delegate temp in toRemove)
                    {
                        _delegateLookup.Remove(temp);
                    }
                }
                _delegates.Remove(typeof(T));
            }
        }*/

        public void Dispatch(FEvent e)
        {
            EventDelegate del;
            if (_delegates.TryGetValue(e.GetType(), out del))
            {
                del.Invoke(e);
            }
        }

        public void DispatchWith<T>()
        {
            DispatchWith<T>(null);
        }

        public void DispatchWith<T>(object data)
        {
            DispatchWith(typeof(T), data);
        }

        public void DispatchWith(Type eventType)
        {
            DispatchWith(eventType, null);
        }

        /// <summary>
        /// Dispatch with a cached event instance of given type
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="data"></param>
        public void DispatchWith(Type eventType, object data)
        {
            FEvent e;
            if (!EventCache.TryGetValue(eventType, out e))
            {
                e = Activator.CreateInstance(eventType, data) as FEvent;
                EventCache.Add(eventType, e);   // Add to cache
            }
            e.Data = data;
            Dispatch(e);
            e.Data = null;
        }
    }
}

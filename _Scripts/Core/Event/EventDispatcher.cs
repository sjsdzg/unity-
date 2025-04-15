using System;
using System.Collections.Generic;
using UnityEngine;

namespace XFramework.Core
{
    public class EventDispatcher : MonoBehaviour
    {
        private static Dictionary<object, Dictionary<string, Delegate>> s_EventTable = new Dictionary<object, Dictionary<string, Delegate>>();
        private static Dictionary<string, Delegate> s_GlobalEventTable = new Dictionary<string, Delegate>();

        private void Awake()
        {
            this.ClearTable();
        }

        private void ClearTable()
        {
            s_EventTable.Clear();
            ExecuteEvent("OnEventHandlerClear");
        }

        internal static void ExecuteEvent<T>(T pLCToWorkshop, object pLCToWorkshop_callBack)
        {
            throw new NotImplementedException();
        }

        public static void ExecuteEvent(string eventName)
        {
            Action action = GetDelegate(eventName) as Action;
            if (action != null)
            {
                action();
            }
        }

        public static void ExecuteEvent(object obj, string eventName)
        {
            Action action = GetDelegate(obj, eventName) as Action;
            if (action != null)
            {
                action();
            }
        }

        public static void ExecuteEvent<T>(string eventName, T arg1)
        {
            Action<T> action = GetDelegate(eventName) as Action<T>;
            if (action != null)
            {
                action(arg1);
            }
        }

        public static void ExecuteEvent<T>(object obj, string eventName, T arg1)
        {
            Action<T> action = GetDelegate(obj, eventName) as Action<T>;
            if (action != null)
            {
                action(arg1);
            }
        }

        public static void ExecuteEvent<T, U>(string eventName, T arg1, U arg2)
        {
            Action<T, U> action = GetDelegate(eventName) as Action<T, U>;
            if (action != null)
            {
                action(arg1, arg2);
            }
        }

        public static void ExecuteEvent<T, U>(object obj, string eventName, T arg1, U arg2)
        {
            Action<T, U> action = GetDelegate(obj, eventName) as Action<T, U>;
            if (action != null)
            {
                action(arg1, arg2);
            }
        }

        public static void ExecuteEvent<T, U, V>(string eventName, T arg1, U arg2, V arg3)
        {
            Action<T, U, V> action = GetDelegate(eventName) as Action<T, U, V>;
            if (action != null)
            {
                action(arg1, arg2, arg3);
            }
        }

        public static void ExecuteEvent<T, U, V>(object obj, string eventName, T arg1, U arg2, V arg3)
        {
            Action<T, U, V> action = GetDelegate(obj, eventName) as Action<T, U, V>;
            if (action != null)
            {
                action(arg1, arg2, arg3);
            }
        }

        public static void ExecuteEvent<T, U, V, W>(string eventName, T arg1, U arg2, V arg3, W arg4)
        {
            Action<T, U, V, W> action = GetDelegate(eventName) as Action<T, U, V, W>;
            if (action != null)
            {
                action(arg1, arg2, arg3, arg4);
            }
        }

        public static void ExecuteEvent<T, U, V, W>(object obj, string eventName, T arg1, U arg2, V arg3, W arg4)
        {
            Action<T, U, V, W> action = GetDelegate(obj, eventName) as Action<T, U, V, W>;
            if (action != null)
            {
                action(arg1, arg2, arg3, arg4);
            }
        }

        private static Delegate GetDelegate(string eventName)
        {
            Delegate delegate2;
            if (s_GlobalEventTable.TryGetValue(eventName, out delegate2))
            {
                return delegate2;
            }
            return null;
        }

        private static Delegate GetDelegate(object obj, string eventName)
        {
            Dictionary<string, Delegate> dictionary;
            Delegate delegate2;
            if (s_EventTable.TryGetValue(obj, out dictionary) && dictionary.TryGetValue(eventName, out delegate2))
            {
                return delegate2;
            }
            return null;
        }

        private void OnDestroy()
        {
            this.ClearTable();
        }

        private void OnDisable()
        {
            if ((base.gameObject == null) || base.gameObject.activeSelf)
            {
                this.ClearTable();
            }
        }

        public static void RegisterEvent<T>(string eventName, Action<T> handler)
        {
            RegisterEvent(eventName, (Delegate)handler);
        }

        public static void RegisterEvent(string eventName, Action handler)
        {
            RegisterEvent(eventName, (Delegate)handler);
        }

        public static void RegisterEvent<T, U>(string eventName, Action<T, U> handler)
        {
            RegisterEvent(eventName, (Delegate)handler);
        }

        public static void RegisterEvent<T, U, V>(string eventName, Action<T, U, V> handler)
        {
            RegisterEvent(eventName, (Delegate)handler);
        }

        public static void RegisterEvent<T, U, V, W>(string eventName, Action<T, U, V, W> handler)
        {
            RegisterEvent(eventName, (Delegate)handler);
        }

        private static void RegisterEvent(string eventName, Delegate handler)
        {
            Delegate delegate2;
            if (s_GlobalEventTable.TryGetValue(eventName, out delegate2))
            {
                s_GlobalEventTable[eventName] = Delegate.Combine(delegate2, handler);
            }
            else
            {
                s_GlobalEventTable.Add(eventName, handler);
            }
        }

        public static void RegisterEvent<T>(object obj, string eventName, Action<T> handler)
        {
            RegisterEvent(obj, eventName, (Delegate)handler);
        }

        public static void RegisterEvent(object obj, string eventName, Action handler)
        {
            RegisterEvent(obj, eventName, (Delegate)handler);
        }

        public static void RegisterEvent<T, U>(object obj, string eventName, Action<T, U> handler)
        {
            RegisterEvent(obj, eventName, (Delegate)handler);
        }

        public static void RegisterEvent<T, U, V>(object obj, string eventName, Action<T, U, V> handler)
        {
            RegisterEvent(obj, eventName, (Delegate)handler);
        }

        public static void RegisterEvent<T, U, V, W>(object obj, string eventName, Action<T, U, V, W> handler)
        {
            RegisterEvent(obj, eventName, (Delegate)handler);
        }

        private static void RegisterEvent(object obj, string eventName, Delegate handler)
        {
            if (obj == null)
            {
                Debug.LogError("EventHandler.RegisterEvent error: target object cannot be null.");
            }
            else
            {
                Dictionary<string, Delegate> dictionary;
                Delegate delegate2;
                if (!s_EventTable.TryGetValue(obj, out dictionary))
                {
                    dictionary = new Dictionary<string, Delegate>();
                    s_EventTable.Add(obj, dictionary);
                }
                if (dictionary.TryGetValue(eventName, out delegate2))
                {
                    dictionary[eventName] = Delegate.Combine(delegate2, handler);
                }
                else
                {
                    dictionary.Add(eventName, handler);
                }
            }
        }

        public static void UnregisterEvent<T>(string eventName, Action<T> handler)
        {
            UnregisterEvent(eventName, (Delegate)handler);
        }

        public static void UnregisterEvent(string eventName, Action handler)
        {
            UnregisterEvent(eventName, (Delegate)handler);
        }

        public static void UnregisterEvent<T, U>(string eventName, Action<T, U> handler)
        {
            UnregisterEvent(eventName, (Delegate)handler);
        }

        public static void UnregisterEvent<T, U, V>(string eventName, Action<T, U, V> handler)
        {
            UnregisterEvent(eventName, (Delegate)handler);
        }

        public static void UnregisterEvent<T, U, V, W>(string eventName, Action<T, U, V, W> handler)
        {
            UnregisterEvent(eventName, (Delegate)handler);
        }

        private static void UnregisterEvent(string eventName, Delegate handler)
        {
            Delegate delegate2;
            if (s_GlobalEventTable.TryGetValue(eventName, out delegate2))
            {
                s_GlobalEventTable[eventName] = Delegate.Remove(delegate2, handler);
            }
        }

        public static void UnregisterAllEvent()
        {
            s_GlobalEventTable.Clear();
            s_EventTable.Clear();
        }

        public static void UnregisterEvent<T>(object obj, string eventName, Action<T> handler)
        {
            UnregisterEvent(obj, eventName, handler);
        }

        public static void UnregisterEvent(object obj, string eventName, Action handler)
        {
            UnregisterEvent(obj, eventName, (Delegate)handler);
        }

        public static void UnregisterEvent<T, U>(object obj, string eventName, Action<T, U> handler)
        {
            UnregisterEvent(obj, eventName, handler);
        }

        public static void UnregisterEvent<T, U, V>(object obj, string eventName, Action<T, U, V> handler)
        {
            UnregisterEvent(obj, eventName, handler);
        }

        public static void UnregisterEvent<T, U, V, W>(object obj, string eventName, Action<T, U, V, W> handler)
        {
            UnregisterEvent(obj, eventName, handler);
        }

        private static void UnregisterEvent(object obj, string eventName, Delegate handler)
        {
            if (obj == null)
            {
                Debug.LogError("EventHandler.UnregisterEvent error: target object cannot be null.");
            }
            else
            {
                Dictionary<string, Delegate> dictionary;
                Delegate delegate2;
                if (s_EventTable.TryGetValue(obj, out dictionary) && dictionary.TryGetValue(eventName, out delegate2))
                {
                    dictionary[eventName] = Delegate.Remove(delegate2, handler);
                }
            }
        }
    }
}


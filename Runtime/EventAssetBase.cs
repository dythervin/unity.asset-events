using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace Dythervin.Events
{
    public abstract class EventAssetBase : ScriptableObject
    {
        public const string MenuName = "Events/";
        private static readonly List<Priority> PriorityList = new List<Priority>() { Priority.High, Priority.Default, Priority.Low };

#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        private readonly LockableListenerContainer<IListener> _listeners = new LockableListenerContainer<IListener>();

        public void Add(IListener value)
        {
            _listeners.Add(value);
        }

        public bool Remove(IListener value)
        {
            return _listeners.Remove(value);
        }

        public bool Contains(IListener value)
        {
            return _listeners.Contains(value);
        }

        protected virtual void OnBeforeInvoke()
        {
            _listeners.Lock(true);
        }

        protected virtual void OnInvoked()
        {
            _listeners.Lock(false);
        }

        protected void RunSimple(Priority priority)
        {
            if (!_listeners.TryGetValue(priority, out var list))
                return;

            foreach (IListener listener in list)
                if (!_listeners.ToRemove(listener))
                    listener.Execute();
        }

        public int Count => _listeners.Count;
        public List<Priority>.Enumerator GetEnumerator() => PriorityList.GetEnumerator();
    }
}
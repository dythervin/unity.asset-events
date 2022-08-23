using System.Collections.Generic;
using Dythervin.Core.Utils;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.Assertions;

namespace Dythervin.Events
{
    public abstract class EventAssetBase : ScriptableObject, IEventContainer<IListener>
    {
        public const string MenuName = "Events/";
        private static readonly List<Priority> PriorityList = new List<Priority>() { Priority.High, Priority.Default, Priority.Low };

#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        private readonly LockableListenerContainer<IListener> _listeners = new LockableListenerContainer<IListener>();

        public void Add(IListener value)
        {
            Assertions.IsNotNull(value);
            _listeners.Add(value);
        }

        public bool Remove(IListener value)
        {
            Assertions.IsNotNull(value);
            return _listeners.Remove(value);
        }

        public bool Contains(IListener value)
        {
            Assertions.IsNotNull(value);
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

        public virtual int Count => _listeners.values.TotalCount;
        public List<Priority>.Enumerator GetEnumerator() => PriorityList.GetEnumerator();
    }
}
using Dythervin.Core.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Dythervin.Events
{
    [CreateAssetMenu(menuName = MenuName + "Simple")]
    public class EventAsset : EventAssetBase, IEvent
    {
        [SerializeField] private UnityEvent onInvoke;

        public event UnityAction OnInvoke;

#if ODIN_INSPECTOR
        [HideInEditorMode, Button]
#endif
        public void Invoke()
        {
            OnBeforeInvoke();
            OnInvoke?.Invoke();
            onInvoke.Invoke();
            if (EventsLogger.Enabled)
                EventsLogger.Log(this, true, Count);

            foreach (Priority priority in this)
            {
                RunSimple(priority);
            }

            if (EventsLogger.Enabled)
                EventsLogger.Log(this, false, Count);
            OnInvoked();
        }
    }

    public class EventAsset<T> : EventAssetBase, IEvent<T>
    {
        [SerializeField] private UnityEvent<T> onInvoke;

#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly] 
#endif
        protected readonly LockableListenerContainer<IListener<T>> listenersGeneric = new LockableListenerContainer<IListener<T>>();

        public override int Count => base.Count + listenersGeneric.values.TotalCount;

        public event UnityAction<T> OnInvoke;

        public void Add(IListener<T> value)
        {
            Assertions.IsNotNull(value);
            listenersGeneric.Add(value);
        }

        public bool Contains(IListener<T> value)
        {
            Assertions.IsNotNull(value);
            return listenersGeneric.Contains(value);
        }

#if ODIN_INSPECTOR
        [HideInEditorMode, Button]
#endif
        public void Invoke(T a)
        {
            OnBeforeInvoke();
            OnInvoke?.Invoke(a);
            onInvoke.Invoke(a);
            if (EventsLogger.Enabled)
                EventsLogger.Log(this, a, true, Count);
            foreach (Priority priority in this)
            {
                RunSimple(priority);
                if (!listenersGeneric.TryGetValue(priority, out var list))
                    continue;

                foreach (var listener in list)
                {
                    if (!listenersGeneric.IsToRemove(listener))
                        listener.Execute(a);
                }
            }

            if (EventsLogger.Enabled)
                EventsLogger.Log(this, a, false, Count);
            OnInvoked();
        }

        public bool Remove(IListener<T> value)
        {
            Assertions.IsNotNull(value);
            return listenersGeneric.Remove(value);
        }

        protected override void OnInvoked()
        {
            base.OnInvoked();
            listenersGeneric.Lock(false);
        }

        protected override void OnBeforeInvoke()
        {
            base.OnBeforeInvoke();
            listenersGeneric.Lock();
        }
    }
}
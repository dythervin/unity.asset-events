#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace Dythervin.Events
{
    [CreateAssetMenu(menuName = MenuName + "Simple")]
    public class EventAsset : EventAssetBase, IEvent
    {
        [SerializeField] private UnityEvent onInvoke;

        public event UnityAction OnInvoke;

#if ODIN_INSPECTOR
        [HideInEditorMode] [Button]
#endif
        public void Invoke()
        {
            OnBeforeInvoke();
            OnInvoke?.Invoke();
            onInvoke.Invoke();
            if (EventsLogger.Enabled)
                EventsLogger.Log(this, true, Count == 0);

            foreach (Priority priority in this)
                RunSimple(priority);

            if (EventsLogger.Enabled)
                EventsLogger.Log(this, false, Count == 0);
            OnInvoked();
        }
    }

    public class EventAsset<T> : EventAssetBase, IEvent<T>
    {
        [SerializeField] private UnityEvent<T> onInvoke;

#if ODIN_INSPECTOR
        [ShowInInspector] [ReadOnly]
#endif
        protected readonly LockableListenerContainer<IListener<T>> listenersGeneric = new LockableListenerContainer<IListener<T>>();

        public event UnityAction<T> OnInvoke;

        public void Add(IListener<T> value)
        {
            listenersGeneric.Add(value);
        }

        public bool Contains(IListener<T> value)
        {
            return listenersGeneric.Contains(value);
        }

#if ODIN_INSPECTOR
        [HideInEditorMode] [Button]
#endif
        public void Invoke(T a)
        {
            OnBeforeInvoke();
            OnInvoke?.Invoke(a);
            onInvoke.Invoke(a);
            if (EventsLogger.Enabled)
                EventsLogger.Log(this, a, true, Count == 0);
            foreach (Priority priority in this)
            {
                RunSimple(priority);
                if (!listenersGeneric.TryGetValue(priority, out var list))
                    continue;

                foreach (var listener in list)
                    if (!listenersGeneric.ToRemove(listener))
                        listener.Execute(a);
            }

            if (EventsLogger.Enabled)
                EventsLogger.Log(this, a, false, Count == 0);
            OnInvoked();
        }

        public bool Remove(IListener<T> value)
        {
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
            listenersGeneric.Lock(true);
        }
    }
}
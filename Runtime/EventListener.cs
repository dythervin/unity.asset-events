#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace Dythervin.Events
{
    public sealed class EventListener : EventListenerBase, IListener
    {
        [SerializeField] private bool raiseOnStart;

#if ODIN_INSPECTOR
        [PropertyOrder(-1)]
#endif
        [SerializeField]
        private EventAssetBase[] events;

        [Space] [SerializeField] private UnityEvent onRaise;

        private void Start()
        {
            if (raiseOnStart)
                ((IListener)this).Execute();
        }


        void IListener.Execute()
        {
            OnRaise();
        }

        private void OnRaise()
        {
            EventsLogger.Log(this);
            onRaise.Invoke();
        }

        protected override void Enabled()
        {
            foreach (EventAssetBase eventAsset in events)
                eventAsset.Add(this);
        }

        protected override void Disabled()
        {
            foreach (EventAssetBase eventAsset in events)
                eventAsset.Remove(this);
        }
    }

    public class EventListener<T> : EventListenerBase, IListener<T>
    {
        [SerializeField]
        private EventAsset<T>[] events;

#if ODIN_INSPECTOR
        [PropertyOrder(int.MaxValue)]
#endif
        [Space]
        [SerializeField]
        private UnityEvent<T> onRaise;

        void IListener<T>.Execute(in T a)
        {
            OnRaise(a);
        }

        protected virtual void OnRaise(in T a)
        {
            EventsLogger.Log(this);
            onRaise.Invoke(a);
        }

        protected override void Enabled()
        {
            foreach (var eventAsset in events)
                eventAsset.Add(this);
        }

        protected override void Disabled()
        {
            foreach (var eventAsset in events)
                eventAsset.Remove(this);
        }
    }
}
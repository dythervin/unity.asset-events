using UnityEngine;
using UnityEngine.Events;

namespace Dythervin.Events
{
    public sealed class EventListener : EventListenerBase<EventAssetBase, UnityEvent>, IListener
    {
        [SerializeField] private bool raiseOnStart;

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

        protected override void Subscribed()
        {
            foreach (EventAssetBase eventAsset in events)
                eventAsset.Add(this);
        }

        protected override void Unsubscribed()
        {
            foreach (EventAssetBase eventAsset in events)
                eventAsset.Remove(this);
        }
    }

    public class EventListener<T> : EventListenerBase<EventAsset<T>, UnityEvent<T>>, IListener<T>
    {
        void IListener<T>.Execute(in T a)
        {
            OnRaise(a);
        }

        protected virtual void OnRaise(in T a)
        {
            EventsLogger.Log(this);
            onRaise.Invoke(a);
        }

        protected override void Subscribed()
        {
            foreach (var eventAsset in events)
                eventAsset.Add(this);
        }

        protected override void Unsubscribed()
        {
            foreach (var eventAsset in events)
                eventAsset.Remove(this);
        }
    }
}
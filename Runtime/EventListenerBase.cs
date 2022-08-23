using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Dythervin.Events
{
    [DefaultExecutionOrder(-1000)]
    public abstract partial class EventListenerBase : MonoBehaviour, IPrioritized
    {
        [EventAdvanced, SerializeField] private Priority priority;
        [EventAdvanced, SerializeField] private Type type;

#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        private bool _subscribed;

        protected virtual void Awake()
        {
            if (type == Type.Awake)
                Subscribe();
        }

        protected virtual void OnEnable()
        {
            if (type == Type.Enable)
                Subscribe();
        }

        protected virtual void OnDisable()
        {
            if (type == Type.Enable)
                Unsubscribe();
        }

        protected virtual void OnDestroy()
        {
            if (type == Type.Awake) // type == Type.Created
                Unsubscribe();
        }

        public Priority Priority => priority;

        protected abstract void Unsubscribed();

        protected abstract void Subscribed();

        private void Unsubscribe()
        {
            if (!_subscribed)
                return;

            Unsubscribed();
            _subscribed = false;
        }


        private void Subscribe()
        {
            if (_subscribed)
                return;

            Subscribed();
            _subscribed = true;
        }

        private enum Type : sbyte
        {
            // [InspectorName("Create-Destroy(Only)")] Created = -1,
            [InspectorName("Enable-Disable")] Enable = 0,
            [InspectorName("Awake-Destroy")] Awake
        }
    }
}
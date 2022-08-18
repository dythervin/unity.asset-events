using Dythervin.Callbacks;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace Dythervin.Events
{
    [DefaultExecutionOrder(-1000)]
    public abstract partial class EventListenerBase : MonoBehaviour, IPrioritized, ISerializationCallbackReceiver, IPlayModeListener
    {
        private enum Type : sbyte
        {
            [InspectorName("Create-Destroy")]
            Created = -1,

            [InspectorName("Enable-Disable")]
            Enable = 0,

            [InspectorName("Awake-Destroy")]
            Awake
        }

        [EventAdvanced] [SerializeField] private Priority priority;
        [EventAdvanced] [SerializeField] private Type type;
        
#if ODIN_INSPECTOR
        [ShowInInspector] [ReadOnly]
 #endif
        private bool _subscribed;

        public Priority Priority => priority;


        protected virtual void Awake()
        {
            if (type == Type.Awake)
                Enable();
        }

        protected virtual void OnDestroy()
        {
            if (type == Type.Awake || type == Type.Created)
                Disable();
        }

        protected virtual void OnDisable()
        {
            if (type == Type.Enable)
                Disable();
        }

        protected virtual void OnEnable()
        {
            if (type == Type.Enable)
                Enable();
        }

        protected abstract void Disabled();

        protected abstract void Enabled();

        private void Disable()
        {
            if (!_subscribed)
                return;

            Disabled();
            _subscribed = false;
        }


        private void Enable()
        {
            if (_subscribed)
                return;

            Enabled();
            _subscribed = true;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (type == Type.Created)
                this.TryEnterPlayMode();
        }

        bool IPlayModeListener.MainThreadOnly => true;

        void IPlayModeListener.OnEnterPlayMode()
        {
            Enable();
        }
    }
}
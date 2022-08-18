using UnityEngine;

namespace Dythervin.Events.Implementations
{
    public class EventListenerBool : EventListener<bool>
    {
        [SerializeField]
        private bool inverse;

        protected override void OnRaise(in bool a)
        {
            base.OnRaise(inverse ? !a : a);
        }
    }
}
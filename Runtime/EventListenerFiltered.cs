using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dythervin.Events
{
    public class EventListenerFiltered<T> : EventListener<T> where T : IEquatable<T>
    {
        [SerializeField] private T desiredValue;

        protected override void OnRaise(in T a)
        {
            if (EqualityComparer<T>.Default.Equals(a, desiredValue))
                base.OnRaise(in a);
        }
    }

    public class EventListenerFilteredObj<T> : EventListener<T> where T : class
    {
        [SerializeField] private T desiredValue;

        protected override void OnRaise(in T a)
        {
            if (ReferenceEquals(a, desiredValue))
                base.OnRaise(in a);
        }
    }
}
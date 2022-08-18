using UnityEngine.Events;

namespace Dythervin.Events
{
    public interface IEventContainer<in T>
    {
        void Add(T value);
        bool Remove(T value);
    }

    public interface IEvent<T> : IEventContainer<IListener<T>>, IEventContainer<IListener>
    {
        event UnityAction<T> OnInvoke;
        void Invoke(T a);
    }

    public interface IEvent : IEventContainer<IListener>
    {
        event UnityAction OnInvoke;
        void Invoke();
    }
}
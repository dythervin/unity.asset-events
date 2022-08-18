namespace Dythervin.Events
{
    public interface IListener<T, T1> : IPrioritized
    {
        void Execute(in T a, in T1 b);
    }

    public interface IListener<T> : IPrioritized
    {
        void Execute(in T a);
    }

    public interface IListener : IPrioritized
    {
        void Execute();
    }
}
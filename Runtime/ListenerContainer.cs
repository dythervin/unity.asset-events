using System.Collections.Generic;
using Dythervin.ObjectPool;

namespace Dythervin.Events
{
    public class ListenerContainer<T> : Dictionary<Priority, HashSet<T>>, ICollection<T>
        where T : IPrioritized
    {
        private static readonly ObjectPoolAuto<HashSet<T>> Pool = new ObjectPoolAuto<HashSet<T>>();

        public int TotalCount
        {
            get
            {
                int count = 0;
                foreach (var value in Values)
                {
                    count += value.Count;
                }

                return count;
            }
        }


        public bool Contains(T value)
        {
            return TryGetValue(value.Priority, out var set) && set.Contains(value);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            foreach (var pair in this)
            {
                foreach (T value in pair.Value)
                {
                    array[arrayIndex++] = value;
                    if (arrayIndex >= array.Length)
                        return;
                }
            }
        }

        bool ICollection<T>.IsReadOnly => ((ICollection<KeyValuePair<Priority, HashSet<T>>>)this).IsReadOnly;

        public void Add(T value)
        {
            Priority priority = value.Priority;
            if (TryGetValue(priority, out var set))
            {
                if (set.Contains(value))
                    return;
            }
            else
            {
                set = Pool.Get();
                Add(priority, set);
            }

            set.Add(value);
        }

        public bool Remove(T value)
        {
            Priority priority = value.Priority;
            if (TryGetValue(priority, out var set))
            {
                if (set.Remove(value))
                {
                    if (set.Count == 0)
                    {
                        Remove(priority);
                        Pool.Release(set, false);
                    }

                    return true;
                }
            }

            return false;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach (var pair in this)
            {
                foreach (T value in pair.Value)
                {
                    yield return value;
                }
            }
        }
    }
}
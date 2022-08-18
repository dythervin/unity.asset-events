using System.Collections.Generic;
using Dythervin.Collections;

namespace Dythervin.Events
{
    public class LockableListenerContainer<T> : LockableCollection<ListenerContainer<T>, T>
        where T : IPrioritized
    {
        public bool TryGetValue(Priority priority, out HashSet<T> value)
        {
            return values.TryGetValue(priority, out value);
        }
    }
}
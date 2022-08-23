using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Dythervin.Events
{
    [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
    public static class EventsLogger
    {
        private static string prefix = string.Empty;
        private static readonly string Space = "   ";

        private const string EventColor = "cyan";
        private const string ListenerColor = "green";
        private const string EmptyColor = "yellow";

        public static bool Enabled => Application.isEditor;


        public static void Log(EventListener listener)
        {
            if (Enabled)
                Debug.Log(Listener(listener), listener);
        }

        public static void Log<T>(EventListener<T> listener)
        {
            if (Enabled)
                Debug.Log($"{Listener(listener)}({typeof(T).Name})", listener);
        }

        public static void Log(EventAsset eventAsset, bool start, int count)
        {
            if (!Enabled)
                return;

            if (start)
                Debug.Log($"{Event(eventAsset)}(){Count(count)}", eventAsset);
            prefix = start
                ? $"{prefix}{Space}"
                : prefix.Remove(prefix.Length - Space.Length);
        }

        public static void Log<T>(EventAsset<T> eventAsset, in T a, bool start, int count)
        {
            if (!Enabled)
                return;

            if (start)
                Debug.Log($"{Event(eventAsset)}({typeof(T).Name} {(a is Object obj ? obj.name : a.ToString())}){Count(count)}", eventAsset);
            prefix = start
                ? $"{prefix}{Space}"
                : prefix.Remove(prefix.Length - Space.Length);
        }

        private static string Event(Object assetEvent)
        {
            return $"{prefix}Event <color={EventColor}>{assetEvent.name}</color> Invoked";
        }

        private static string Listener(Object listener)
        {
            return $"{prefix}Listener <color={ListenerColor}>{listener.name}</color> Raised";
        }

        private static string Count(int value)
        {
            return $" (<color={EmptyColor}>{value.ToString()}</color>)";
        }
    }
}
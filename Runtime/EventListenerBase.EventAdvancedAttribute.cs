#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Dythervin.Events
{
    public abstract partial class EventListenerBase
    {
        private class EventAdvancedAttribute :
#if ODIN_INSPECTOR
            FoldoutGroupAttribute
#else
            System.Attribute
#endif
        {
            private const string BoxName = "Advanced";

            public EventAdvancedAttribute(float order = 0)
#if ODIN_INSPECTOR
                : base(BoxName, order)
#endif
            { }

            public EventAdvancedAttribute(bool expanded, float order = 0)
#if ODIN_INSPECTOR
                : base(BoxName, expanded, order)
#endif
            { }
        }
    }
}
using UnityEngine;

namespace Dythervin.Events.Implementations
{
    [CreateAssetMenu(menuName = MenuName + "Bool")]
    public class EventAssetBool : EventAsset<bool>
    {
        public void InvokeInverse(bool a)
        {
            Invoke(!a);
        }
    }
}
using UnityEngine;

namespace Dythervin.Events.Implementations
{
    [CreateAssetMenu(menuName = MenuName + "Float")]
    public class EventAssetFloat : EventAsset<(int, bool)> { }
}
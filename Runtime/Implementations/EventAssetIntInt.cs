using UnityEngine;

namespace Dythervin.Events.Implementations
{
    [CreateAssetMenu(menuName = MenuName + "Int Int")]
    public class EventAssetIntInt : EventAsset<(int, int)> { }
}
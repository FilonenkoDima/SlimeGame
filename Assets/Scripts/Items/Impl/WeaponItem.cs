using Items.Core;
using UnityEngine;

namespace Items.Impl
{
    [CreateAssetMenu(fileName = "WeaponItem", menuName = "Items/WeaponItem", order = 0)]
    public class WeaponItem : EquipmentItem
    {
        [field: SerializeField] public float AttackAmount { private set; get; }

        public override EquipmentType GetType(ItemContainer itemContainer)
        {
            return EquipmentType.Weapon;
        }
    }
}
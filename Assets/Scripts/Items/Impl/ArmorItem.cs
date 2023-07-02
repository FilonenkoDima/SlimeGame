using Items.Core;
using Player;
using UnityEngine;

namespace Items.Impl
{
    [CreateAssetMenu(fileName = "ArmorItem", menuName = "Items/ArmorItem", order = 0)]
    public class ArmorItem : EquipmentItem
    {
        [field: SerializeField] public EquipmentType EquipmentType { protected set; get; } = EquipmentType.None;
        [field: SerializeField] public float ArmorAmount { private set; get; }

        public override EquipmentType GetType(ItemContainer itemContainer)
        {
            return EquipmentType;
        }

        public void OnEquipped(PlayerEntity playerEntity, ItemContainer itemContainer)
        {
            Debug.Log("OnEquipped");
        }

        public void OnUnequipped(PlayerEntity playerEntity, ItemContainer itemContainer)
        {
            Debug.Log("OnUnequipped");
        }
    }
}
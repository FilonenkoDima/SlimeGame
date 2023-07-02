using Items.Core;
using Items.Impl;

namespace Items.InventoryImpl
{
    public class EquipmentInventory : Inventory
    {
        private readonly EquipmentType[] _equipmentTypes;

        public EquipmentInventory(EquipmentType[] equipmentTypes) : base(equipmentTypes.Length)
        {
            _equipmentTypes = equipmentTypes;
        }

        public override ItemContainer InsertItem(int index, ItemContainer item, bool simulate)
        {
            var inSlotType = _equipmentTypes[index];

            var goodEquip = item.Item is EquipmentItem equipmentItem && equipmentItem.GetType(item) == inSlotType;
            return goodEquip ? base.InsertItem(index, item, simulate) : item;
        }

        public ItemContainer GetItemByType(EquipmentType equipmentType)
        {
            for (var i = 0; i < _equipmentTypes.Length; i++)
            {
                var inSlotType = _equipmentTypes[i];
                if (inSlotType == equipmentType)
                {
                    return GetItem(i);
                }
            }

            return null;
        }
    }
}
using Items.Core;
using Player;

namespace Items.Impl
{
    public abstract class EquipmentItem : BaseItem
    {
        public abstract EquipmentType GetType(ItemContainer itemContainer);

        public override void OnUse(PlayerEntity playerEntity, ItemContainer itemContainer)
        {
        }

        public override int GetMaxStack(ItemContainer itemContainer)
        {
            return 1;
        }
    }
}
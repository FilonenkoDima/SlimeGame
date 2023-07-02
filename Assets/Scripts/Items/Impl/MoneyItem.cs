using Items.Core;
using Player;
using UnityEngine;

namespace Items.Impl
{
    [CreateAssetMenu(fileName = "MoneyItem", menuName = "Items/MoneyItem", order = 0)]
    public class MoneyItem : BaseItem
    {
        [field: SerializeField] public int MoneyPoints { private set; get; }

        public override int GetMaxStack(ItemContainer itemContainer)
        {
            return 0;
        }

        public override bool TryToPickup(PlayerEntity playerEntity, ItemContainer itemContainer)
        {
            playerEntity.AddMoney(MoneyPoints);
            return true;
        }
    }
}
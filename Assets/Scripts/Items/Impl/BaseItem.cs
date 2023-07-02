using Items.Core;
using Player;
using UnityEngine;

namespace Items.Impl
{
    [CreateAssetMenu(fileName = "BaseItem", menuName = "Items/BaseItem", order = 0)]
    public class BaseItem : Item
    {
        [SerializeField] private ItemId id;
        [SerializeField] private string baseName;
        [SerializeField] private string baseDescription;
        [SerializeField] private Sprite baseSprite;
        [SerializeField] private ItemRarity baseRarity;
        [field: SerializeField] protected int BaseMaxStack { private set; get; } = 64;

        public override ItemId GetId() => id;

        public override ItemRarity GetRarity(ItemContainer itemContainer) => baseRarity;
        public override Sprite GetSprite(ItemContainer itemContainer) => baseSprite;
        public override string GetName(ItemContainer itemContainer) => baseName;
        public override string GetDescription(ItemContainer itemContainer) => baseDescription;
        public override int GetMaxDurability(ItemContainer itemContainer) => -1;
        public override int GetMaxStack(ItemContainer itemContainer) => BaseMaxStack;

        public override void OnUse(PlayerEntity playerEntity, ItemContainer itemContainer)
        {
            Debug.Log($"USE WITH ITEM: {itemContainer.Item.GetId()}");
        }

        public override bool TryToPickup(PlayerEntity playerEntity, ItemContainer itemContainer)
        {
            var restStack = playerEntity.Inventory.AddItem(itemContainer, true);
            if (restStack != null)
                return false;

            playerEntity.Inventory.AddItem(itemContainer, false);

            return true;
        }
    }
}
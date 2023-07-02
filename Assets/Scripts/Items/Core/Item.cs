using Player;
using UnityEngine;

namespace Items.Core
{
    public abstract class Item : ScriptableObject
    {
        public abstract ItemId GetId();
        public abstract ItemRarity GetRarity(ItemContainer itemContainer);
        public abstract Sprite GetSprite(ItemContainer itemContainer);
        public abstract string GetName(ItemContainer itemContainer);
        public abstract string GetDescription(ItemContainer itemContainer);
        public abstract int GetMaxDurability(ItemContainer itemContainer);
        public abstract int GetMaxStack(ItemContainer itemContainer);
        public abstract void OnUse(PlayerEntity playerEntity, ItemContainer itemContainer);
        
        /**
         * return true if item was picked up, otherwise - false
         **/
        public abstract bool TryToPickup(PlayerEntity playerEntity, ItemContainer itemContainer);

    }
}
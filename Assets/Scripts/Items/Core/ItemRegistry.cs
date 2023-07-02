using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Items.Core
{
    [CreateAssetMenu(menuName = "Registries", fileName = "ItemRegistry", order = 0)]
    public class ItemRegistry : ScriptableObject
    {
        [SerializeField] private List<Item> items;

        public Item GetItemById(ItemId itemId)
        {
            var item = items.Find(item => item.GetId() == itemId);
            if (item == null)
            {
                throw new DataException($"Item with id: {itemId} was not registered!!!");
            }
            return item;
        }
    }
}
using System;
using System.Collections.Generic;

namespace Items.Core
{
    public class Inventory
    {
        private readonly ItemContainer[] _itemsArray;

        public event Action<Dictionary<int, ItemContainer>> ItemsArrayChanged;

        public Inventory(int size)
        {
            _itemsArray = new ItemContainer[size];
        }

        public ItemContainer GetItem(int index)
        {
            return _itemsArray[index];
        }

        public int GetSize()
        {
            return _itemsArray.Length;
        }

        public void SetItem(int index, ItemContainer item)
        {
            SetItemsBulk(new Dictionary<int, ItemContainer> { { index, item } });
        }

        public void SetItemsBulk(Dictionary<int, ItemContainer> items)
        {
            foreach (var keyValuePair in items)
            {
                _itemsArray[keyValuePair.Key] = keyValuePair.Value;
                ItemsArrayChanged?.Invoke(items);
            }
        }

        /* Returns new item with same type, but with count of the rest of the stack or null if item fully inserted*/
        public virtual ItemContainer InsertItem(int index, ItemContainer item, bool simulate)
        {
            if (item == null)
            {
                return null;
            }

            var inSlot = GetItem(index);

            if (inSlot == null)
            {
                if (!simulate)
                {
                    SetItem(index, item);
                }

                return null;
            }

            var toAdd = item.Clone();
            var maxStack = toAdd.Item.GetMaxStack(toAdd);

            if (toAdd.IsItemEqual(inSlot))
            {
                var canAdd = Math.Min(toAdd.Amount, maxStack - inSlot.Amount);
                if (canAdd > 0)
                {
                    toAdd.SetAmount(toAdd.Amount - canAdd);
                    if (!simulate)
                    {
                        inSlot.SetAmount(inSlot.Amount + canAdd);
                        ItemsArrayChanged?.Invoke(new Dictionary<int, ItemContainer> { { index, inSlot } });
                    }
                }
            }

            return toAdd.Amount > 0 ? toAdd : null;
        }

        /* Do the same like InsertItem, but automatically try to fit item in inventory*/
        public ItemContainer AddItem(ItemContainer item, bool simulate)
        {
            var restOfItem = item.Clone();

            for (var i = 0; i < _itemsArray.Length; i++)
            {
                restOfItem = InsertItem(i, restOfItem, simulate);

                if (restOfItem == null)
                {
                    break;
                }
            }

            return restOfItem;
        }

        public void RemoveItem(ItemContainer item)
        {
            for (var i = 0; i < _itemsArray.Length; i++)
            {
                var inSlot = GetItem(i);
                if (inSlot != item) continue;

                SetItem(i, null);
                break;
            }
        }
    }
}
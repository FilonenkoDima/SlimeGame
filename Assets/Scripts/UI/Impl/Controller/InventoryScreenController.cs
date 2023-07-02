using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Items.Core;
using Player;
using Ui.Core;
using UI.Core;
using UI.Enum;
using UI.Impl.View;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Impl.Controller
{
    public class InventoryScreenController : ScreenController<InventoryScreenView>
    {
        private readonly SceneItemsSystem _sceneItemsSystem;
        private readonly PlayerEntity _playerEntity;

        private readonly Inventory _backPackInventory;
        private readonly Inventory _equipmentInventory;

        private List<UISlot> _backpackSlots;
        private List<UISlot> _equipmentSlots;

        private UISlot _draggingSourceSlot;

        public InventoryScreenController(UIContext uiContext, InventoryScreenView view, PlayerEntity playerEntity,
            SceneItemsSystem sceneItemsSystem)
            : base(uiContext, view)
        {
            _sceneItemsSystem = sceneItemsSystem;
            _playerEntity = playerEntity;
            _backPackInventory = playerEntity.Inventory;
            _equipmentInventory = playerEntity.EquipmentInventory;
        }

        public override void OnInit()
        {
            _backpackSlots = View.BackpackSlotsHolder.GetComponentsInChildren<UISlot>().ToList();
            _equipmentSlots = View.EquipmentSlotsHolder.GetComponentsInChildren<UISlot>().ToList();
            if (_backpackSlots.Count != _backPackInventory.GetSize())
            {
                Debug.LogError("Amount of backpack slots is not equal to inv size!");
            }

            if (_equipmentSlots.Count != _equipmentInventory.GetSize())
            {
                Debug.LogError("Amount of equipment slots is not equal to inv size!");
            }

            View.OnCloseButton += () => { UIContext.CloseScreen(); };

            _backpackSlots.ForEach(InitSlotEvents);
            _equipmentSlots.ForEach(InitSlotEvents);
        }

        public override void OnOpenRequest()
        {
            View.MovingImage.SetItemImage(null);

            InitializeItemsInSlots();

            _backPackInventory.ItemsArrayChanged += UpdateBackpackSlots;
            _equipmentInventory.ItemsArrayChanged += UpdateEquipmentSlots;

            base.OnOpenRequest();
        }

        public override void OnCloseRequest()
        {
            ClearItemsInSlots();

            _backPackInventory.ItemsArrayChanged -= UpdateBackpackSlots;
            _equipmentInventory.ItemsArrayChanged -= UpdateEquipmentSlots;

            base.OnCloseRequest();
        }

        private void InitializeItemsInSlots()
        {
            for (var i = 0; i < _backpackSlots.Count; i++)
            {
                var slot = _backpackSlots[i];
                var item = _backPackInventory.GetItem(i);

                InitSlotEvents(slot);
                ApplyItemToUISlot(slot, item);
            }

            for (var i = 0; i < _equipmentSlots.Count; i++)
            {
                var slot = _equipmentSlots[i];
                var item = _equipmentInventory.GetItem(i);

                InitSlotEvents(slot);
                ApplyItemToUISlot(slot, item);
            }
        }

        private void ClearItemsInSlots()
        {
            _backpackSlots.ForEach(slot => slot.ClearItem());
            _equipmentSlots.ForEach(slot => slot.ClearItem());
        }

        private void UpdateBackpackSlots(IReadOnlyDictionary<int, ItemContainer> items) =>
            UpdateChangedSlots(_backpackSlots, items);

        private void UpdateEquipmentSlots(IReadOnlyDictionary<int, ItemContainer> items) =>
            UpdateChangedSlots(_equipmentSlots, items);

        private void InitSlotEvents(UISlot slot)
        {
            slot.SlotDragStart += _ => StartDragSlot(slot);
            slot.SlotDragging += data => DraggingSlot(data, slot);
            slot.SlotDragEnd += data => StopDragSlot(data, slot);
        }

        private void StartDragSlot(UISlot slot)
        {
            var draggingSourceItem = GetItemInSlot(slot);

            if (draggingSourceItem == null)
                return;

            var texture = draggingSourceItem.Item.GetSprite(draggingSourceItem);
            View.MovingImage.SetItemImage(texture);

            slot.ClearItem();
            _draggingSourceSlot = slot;
        }

        private void DraggingSlot(PointerEventData data, UISlot slot)
        {
            if (_draggingSourceSlot != null)
            {
                View.MovingImage.gameObject.transform.position = data.position;
            }
        }

        private void StopDragSlot(PointerEventData data, UISlot slot)
        {
            if (_draggingSourceSlot == null)
                return;

            var movingItem = GetItemInSlot(_draggingSourceSlot);
            if (movingItem == null)
                return;

            var (slotOverMouse, zoneOverMouse) = GetSlotOrDropZoneOverMouse(data);

            var successfullyDragged = false;
            if (slotOverMouse == null && zoneOverMouse != null)
            {
                _sceneItemsSystem.DropItem(movingItem, _playerEntity.transform.position);
                successfullyDragged = true;
            }
            else if (slotOverMouse != null && slotOverMouse != _draggingSourceSlot)
            {
                var restStack = InsertItemInSlot(slotOverMouse, movingItem, true);

                if (restStack == null)
                {
                    InsertItemInSlot(slotOverMouse, movingItem, false);
                    ApplyItemToUISlot(slotOverMouse, movingItem);

                    successfullyDragged = true;
                }
            }

            if (!successfullyDragged)
            {
                ApplyItemToUISlot(_draggingSourceSlot, movingItem);
            }
            else
            {
                SetItemInSlot(_draggingSourceSlot, null);
            }

            View.MovingImage.SetItemImage(null);
            _draggingSourceSlot = null;
        }

        private static void UpdateChangedSlots(
            IReadOnlyList<UISlot> slots,
            IReadOnlyDictionary<int, ItemContainer> items
        )
        {
            foreach (var (index, item) in items)
            {
                var slot = slots[index];
                ApplyItemToUISlot(slot, item);
            }
        }

        private static void ApplyItemToUISlot(UISlot slot, [AllowNull] ItemContainer item)
        {
            if (item != null)
            {
                slot.SetItem(item.Item.GetSprite(item), null, item.Amount);
            }
            else
            {
                slot.ClearItem();
            }
        }

        private static Tuple<UISlot, DraggableDropZone> GetSlotOrDropZoneOverMouse(PointerEventData pointerEventData)
        {
            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            foreach (var result in raycastResults)
            {
                if (result.gameObject.TryGetComponent<UISlot>(out var slot))
                {
                    return new Tuple<UISlot, DraggableDropZone>(slot, null);
                }
            }

            if (raycastResults.Count == 1)
            { // only if background
                foreach (var result in raycastResults)
                {
                    if (result.gameObject.TryGetComponent<DraggableDropZone>(out var zone))
                    {
                        return new Tuple<UISlot, DraggableDropZone>(null, zone);
                    }
                }
            }

            return new Tuple<UISlot, DraggableDropZone>(null, null);
        }

        private ItemContainer GetItemInSlot(UISlot slot)
        {
            if (_backpackSlots.Contains(slot))
            {
                var index = _backpackSlots.IndexOf(slot);
                return _backPackInventory.GetItem(index);
            }

            if (_equipmentSlots.Contains(slot))
            {
                var index = _equipmentSlots.IndexOf(slot);
                return _equipmentInventory.GetItem(index);
            }

            return null;
        }

        private ItemContainer InsertItemInSlot(UISlot slot, ItemContainer item, bool simulate)
        {
            if (_backpackSlots.Contains(slot))
            {
                var index = _backpackSlots.IndexOf(slot);
                return _backPackInventory.InsertItem(index, item, simulate);
            }

            if (_equipmentSlots.Contains(slot))
            {
                var index = _equipmentSlots.IndexOf(slot);
                return _equipmentInventory.InsertItem(index, item, simulate);
            }

            return item;
        }

        private void SetItemInSlot(UISlot slot, ItemContainer item)
        {
            if (_backpackSlots.Contains(slot))
            {
                var index = _backpackSlots.IndexOf(slot);
                _backPackInventory.SetItem(index, item);
            }

            if (_equipmentSlots.Contains(slot))
            {
                var index = _equipmentSlots.IndexOf(slot);
                _equipmentInventory.SetItem(index, item);
            }
        }

        public override ScreenType GetScreenType() => ScreenType.Inventory;
    }
}
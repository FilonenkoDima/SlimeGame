using System;
using System.Collections.Generic;
using System.Linq;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Items.Core
{
    public class SceneItemsSystem : IDisposable
    {
        private readonly Dictionary<SceneItem, ItemContainer> _itemsOnScene;
        private readonly SceneItem _prefab;
        private readonly GameObject _parentObject;
        private readonly PlayerEntity _playerEntity;

        public static SceneItemsSystem Instance { get; private set; }

        public SceneItemsSystem(PlayerEntity playerEntity)
        {
            _playerEntity = playerEntity;
            _prefab = Resources.Load<SceneItem>($"Prefabs/{nameof(SceneItemsSystem)}/{nameof(SceneItem)}");

            _itemsOnScene = new Dictionary<SceneItem, ItemContainer>();
            _parentObject = new GameObject
            {
                name = nameof(SceneItemsSystem)
            };

            Instance = this;
        }

        public void DropItem(ItemContainer itemContainer, Vector2 position)
        {
            var item = itemContainer.Item;

            var created = Object.Instantiate(_prefab, _parentObject.transform);

            created.transform.position = position;
            created.SetItem(
                item.GetSprite(itemContainer),
                item.GetName(itemContainer),
                Color.white //todo color system
            );

            created.PlayDropAnimation(position);
            created.RegisterPickupAction(TryToPickupItems);

            _itemsOnScene[created] = itemContainer;
        }

        public void DropRandomItem(List<SerializedDropItemContainer> items, Vector2 pos)
        {
            if (items is not { Count: > 0 }) return;
            
            var item = items[Random.Range(0, items.Count)];
            if (item == null) return;

            var minItemAmount = item.MinAmount;
            var maxItemAmount = item.MaxAmount;
            var itemAmount = Random.Range(minItemAmount, maxItemAmount);

            if (itemAmount <= 0) return;
            var itemContainer = new ItemContainer(item.Item, itemAmount);

            DropItem(itemContainer, pos);
        }

        private void TryToPickupItems(SceneItem sceneItem)
        {
            if (!sceneItem.CanBePickedUp()) return;

            var container = _itemsOnScene[sceneItem];

            var successful = container.Item.TryToPickup(_playerEntity, container);

            if (!successful) return;

            DetachSceneItem(sceneItem);
        }

        private void DetachSceneItem(SceneItem sceneItem)
        {
            sceneItem.UnregisterPickupAction(TryToPickupItems);
            _itemsOnScene.Remove(sceneItem);
            if (!sceneItem.IsDestroyed())
            {
                Object.Destroy(sceneItem.gameObject);
            }
        }

        public void Dispose()
        {
            foreach (var sceneItem in _itemsOnScene.Keys.ToList())
            {
                DetachSceneItem(sceneItem);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using Drawing;
using NPC.Controller;
using NPC.Data;
using NPC.Enum;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NPC.Spawn
{
    public class EntitySpawnerController : IDisposable
    {
        public static EntitySpawnerController Instance { get; private set; }
        private readonly LevelDrawer _levelDrawer;
        private readonly List<Entity> _entities;
        private readonly EntitiesFactory _entitiesFactory;

        public EntitySpawnerController(LevelDrawer levelDrawer)
        {
            Instance = this;
            _levelDrawer = levelDrawer;
            _entities = new List<Entity>();
            var entitiesSpawnerDataStorage = Resources.Load<EntitiesSpawnerDataStorage>(
                $"{nameof(EntitySpawnerController)}/{nameof(EntitiesSpawnerDataStorage)}");
            _entitiesFactory = new EntitiesFactory(entitiesSpawnerDataStorage);
        }

        public void SpawnEntity(EntityId entityId, Vector2 position)
        {
            var entity = _entitiesFactory.GetEntityBrain(entityId, position);
            _levelDrawer.RegisterElement(entity);
            _entities.Add(entity);
        }

        public void SpawnRandomEntity(List<EntityId> entities, Vector2 pos)
        {
            if (entities is not { Count: > 0 }) return;

            var entity = entities[Random.Range(0, entities.Count)];

            SpawnEntity(entity, pos);
        }

        public void Dispose()
        {
            foreach (var entity in _entities)
                DestroyEntity(entity);
            _entities.Clear();
        }

        private void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
            DestroyEntity(entity);
        }

        private void DestroyEntity(Entity entity)
        {
            _levelDrawer.UnregisterElement(entity);
        }
    }
}
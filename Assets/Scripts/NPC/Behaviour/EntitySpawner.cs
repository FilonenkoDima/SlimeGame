using System.Collections;
using System.Collections.Generic;
using NPC.Enum;
using NPC.Spawn;
using UnityEngine;

namespace NPC.Behaviour
{
    public class EntitySpawner : MonoBehaviour
    {
        [SerializeField] private List<EntityId> entities;
       
        [SerializeField] private float initialDelay = 5.0f;
        [SerializeField] private float respawnTime = 1.0f;
        [SerializeField] private int maxSpawnIterations = 1;
        
        void Start()
        {
            StartCoroutine(EntitiesSpawn());
        }

        private IEnumerator EntitiesSpawn()
        {
            yield return new WaitForSeconds(initialDelay);
            var i = 0;
            while (maxSpawnIterations > i)
            {
                EntitySpawnerController.Instance.SpawnRandomEntity(entities, transform.position);
                i++;
                yield return new WaitForSeconds(respawnTime);
            }

            Destroy(this);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Items.Core;
using UnityEngine;

namespace Items
{
    public class ItemSpawn : MonoBehaviour
    {
        [SerializeField] private List<SerializedDropItemContainer> items;
        [SerializeField] private float initialDelay = 5.0f;
        [SerializeField] private float respawnTime = 1.0f;
        [SerializeField] private int maxSpawnIterations = 1;

        void Start()
        {
            StartCoroutine(ItemsSpawn());
        }


        private IEnumerator ItemsSpawn()
        {
            yield return new WaitForSeconds(initialDelay);
            var i = 0;
            while (maxSpawnIterations > i)
            {
                SceneItemsSystem.Instance.DropRandomItem(items, transform.position);
                i++;
                yield return new WaitForSeconds(respawnTime);
            }

            Destroy(this);
        }
    }
}
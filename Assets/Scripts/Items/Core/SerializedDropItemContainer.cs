using System;
using UnityEngine;

namespace Items.Core
{
    [Serializable]
    public class SerializedDropItemContainer
    {
        [field: SerializeField] public Item Item { get; private set; }
        [field: SerializeField] public int MinAmount { get; private set; } = 1;
        [field: SerializeField] public int MaxAmount { get; private set; } = 1;
    }
}
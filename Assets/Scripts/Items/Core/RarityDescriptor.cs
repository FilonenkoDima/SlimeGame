using UnityEngine;

namespace Items.Core
{
    public class RarityDescriptor
    {
        [field: SerializeField] public ItemRarity ItemRarity { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
    }
}
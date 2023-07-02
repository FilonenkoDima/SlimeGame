using UnityEngine;

namespace Player
{
    public class PlayerArmorUpdater : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer head;
        [SerializeField] private SpriteRenderer armor;
        [SerializeField] private SpriteRenderer shield;
        [SerializeField] private SpriteRenderer weapon;

        private PlayerEntity _playerEntity;

        private void Awake()
        {
            _playerEntity = GetComponent<PlayerEntity>();
        }

        private void FixedUpdate()
        {
            var armorInv = _playerEntity.EquipmentInventory;
            var headItem = armorInv.GetItemByType(EquipmentType.Helmet);
            var armorItem = armorInv.GetItemByType(EquipmentType.Armor);
            var shieldItem = armorInv.GetItemByType(EquipmentType.Shield);
            var weaponItem = armorInv.GetItemByType(EquipmentType.Weapon);


            head.sprite = headItem?.Item.GetSprite(headItem);
            armor.sprite = armorItem?.Item.GetSprite(armorItem);
            shield.sprite = shieldItem?.Item.GetSprite(shieldItem);
            weapon.sprite = weaponItem?.Item.GetSprite(weaponItem);
        }
    }
}
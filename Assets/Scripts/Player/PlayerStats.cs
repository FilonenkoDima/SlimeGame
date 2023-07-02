using System;
using NPC.Behaviour;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    [RequireComponent(typeof(PlayerEntity))]
    [RequireComponent(typeof(EntityCanBeAttacked))]
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private Mask hpMask;
        [SerializeField] private TMP_Text moneyText;
        [SerializeField] private TMP_Text xpText;

        private float _prevHealth;
        private float _prevMaxHealth;
        private PlayerEntity _playerEntity;
        private EntityCanBeAttacked _entityCanBeAttacked;
        private RectTransform _maskParentTransform;

        private void Awake()
        {
            _playerEntity = GetComponent<PlayerEntity>();
            _entityCanBeAttacked = GetComponent<EntityCanBeAttacked>();
            _maskParentTransform = hpMask.rectTransform.parent.GetComponent<RectTransform>();
        }

        public void FixedUpdate()
        {
            moneyText.text = _playerEntity.Money.ToString();
            xpText.text = _playerEntity.Xp.ToString() + " / " + _playerEntity.XpForNextLevel;
            if (_prevHealth != _entityCanBeAttacked.Health || _prevMaxHealth != _entityCanBeAttacked.MaxHealth)
            {
                var parentH = _maskParentTransform.rect.height;
                var height = Math.Min(1F, _entityCanBeAttacked.Health / _entityCanBeAttacked.MaxHealth) * parentH;
                hpMask.rectTransform.anchoredPosition = new Vector2(0, -(parentH - height) / 2);
                hpMask.rectTransform.sizeDelta = new Vector2(hpMask.rectTransform.sizeDelta.x, height);
                _prevHealth = _entityCanBeAttacked.Health;
                _prevMaxHealth = _entityCanBeAttacked.MaxHealth;

                PlayerPrefs.SetFloat("Hp", _prevHealth);
            }
        }
    }
}
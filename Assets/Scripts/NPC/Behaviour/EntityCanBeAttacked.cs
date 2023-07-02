using System;
using UnityEngine;

namespace NPC.Behaviour
{
    public class EntityCanBeAttacked : MonoBehaviour
    {
        [field: SerializeField] public float MaxHealth { get; private set; } = 20;

        [field: SerializeField] public float Health { get; set; } = 10;
        [SerializeField] public float Armor;

        public event Action OnDeath;
        public event Action AfterAttacked;


        public void SetHealth(float health)
        {
            Health = Math.Min(Math.Max(0, health), MaxHealth);
        }

        public void Heal(float health)
        {
            SetHealth(Health + health);
        }

        public virtual void OnAttack(GameObject source, float amount)
        {
            var damage = amount - amount * (Armor / 100);
            if (damage <= 0) return;

            SetHealth(Health - damage);
            AfterAttacked?.Invoke();

            if (Health == 0)
            {
                OnDeath?.Invoke();
            }
        }
    }
}
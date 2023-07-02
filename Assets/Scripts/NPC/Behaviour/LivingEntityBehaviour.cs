using System.Collections.Generic;
using Items.Core;
using Player;
using UnityEngine;

namespace NPC.Behaviour
{
    [RequireComponent(typeof(EntityCanBeAttacked))]
    [RequireComponent(typeof(EntityHpBar))]
    public class LivingEntityBehaviour : BaseEntityBehaviour
    {
        [SerializeField] private List<SerializedDropItemContainer> drop;
        [SerializeField] private int xp;
        [SerializeField] private EntityHpBar entityHpBar;
        private EntityCanBeAttacked _entityCanBeAttacked;

        public override void Awake()
        {
            base.Awake();
            _entityCanBeAttacked = GetComponent<EntityCanBeAttacked>();
        }

        private void Start()
        {
            _entityCanBeAttacked.AfterAttacked += AfterAttacked;
            _entityCanBeAttacked.OnDeath += OnDeath;
        }

        public virtual void OnDestroy()
        {
            _entityCanBeAttacked.AfterAttacked -= AfterAttacked;
            _entityCanBeAttacked.OnDeath -= OnDeath;
        }

        public void OnDeath()
        {
            SceneItemsSystem.Instance.DropRandomItem(drop, transform.position);
            PlayerEntity.CurrentPlayer.AddXp(xp);
            Destroy(gameObject);
        }

        protected virtual void AfterAttacked()
        {
            entityHpBar.UpdateHp(_entityCanBeAttacked.Health, _entityCanBeAttacked.MaxHealth);
        }
    }
}
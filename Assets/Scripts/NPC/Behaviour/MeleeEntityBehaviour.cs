using System.Collections;
using Core.Animation;
using Core.Movement.Controller;
using Core.Services.Updater;
using Player;
using UnityEngine;

namespace NPC.Behaviour
{
    public class MeleeEntityBehaviour : LivingEntityBehaviour
    {
        [SerializeField] private float afterAttackCooldown = 0.4F;
        [SerializeField] private int attackDamage = 10;
        [SerializeField] private float attackDistance = 1.5f;
        [SerializeField] private float speed = 1f;
        [SerializeField] private float stunTimeSeconds = 1f;

        private bool _isAttacking;
        private bool _isStunned;

        public override void Awake()
        {
            base.Awake();
            DirectionalMover = new PositionMover(Rigidbody);
            ProjectUpdater.Instance.FixedUpdateCalled += OnFixedUpdateCalled;
        }

        private void Update() => UpdateAnimations();

        private void OnFixedUpdateCalled()
        {
            if (_isAttacking)
            {
                return;
            }

            var target = PlayerEntity.CurrentPlayer;
            var targetGameObject = target.gameObject;
            var targetPos = targetGameObject.transform.position;

            var entityPos = transform.position;
            var deltaTargets = targetPos - entityPos;
            var distance = deltaTargets.magnitude;
            if (distance > attackDistance)
            {
                var moveVector = deltaTargets.normalized;

                var newPos = entityPos + moveVector * Time.deltaTime * speed * (_isStunned ? 0.5F : 1F);

                var movementAmount = newPos - entityPos;


                if (movementAmount.x != 0)
                {
                    MoveHorizontally(newPos.x);
                }
                else if (movementAmount.y != 0)
                {
                    MoveVertically(newPos.y);
                    EntityLinked.OnVerticalPositionChanged();
                }
            }
            else
            {
                StartAttack(target);
            }
        }
        
        public override void OnDestroy()
        {
            base.OnDestroy();
            ProjectUpdater.Instance.FixedUpdateCalled -= OnFixedUpdateCalled;
        }
        
        private void StartAttack(PlayerEntity target)
        {
            _isAttacking = true;
            Attack(target, attackDamage);
            ProjectUpdater.Instance.StartCoroutine(EndAttack());
        }

        private void Attack(Component target, float amount)
        {
            var entityWithHealth = target.GetComponent<EntityCanBeAttacked>();
            entityWithHealth.OnAttack(this.gameObject, amount);
        }

        private IEnumerator EndAttack()
        {
            yield return new WaitForSeconds(afterAttackCooldown);
            _isAttacking = false;
        }

        protected override void UpdateAnimations()
        {
            base.UpdateAnimations();
            animator.SetAnimationState(AnimationType.Attack, _isAttacking);
        }
        
        protected override void AfterAttacked()
        {
            base.AfterAttacked();
            ProjectUpdater.Instance.StartCoroutine(StanningCoroutine());
        }

        private IEnumerator StanningCoroutine()
        {
            _isStunned = true;
            yield return new WaitForSeconds(stunTimeSeconds);
            _isStunned = false;
        }
    }
}
using Core.Animation;
using Core.Enums;
using Core.Movement.Controller;
using Core.Movement.Data;
using Core.Tools;
using Items.Core;
using Items.Impl;
using Items.InventoryImpl;
using NPC.Behaviour;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(EntityCanBeAttacked))]
    public class PlayerEntity : BaseEntityBehaviour
    {
        public static PlayerEntity CurrentPlayer { private set; get; }

        [SerializeField] private DirectionalMovementData _directionMovementData;
        [SerializeField] private JumpData _jumpData;
        [SerializeField] private DirectionalCameraPair _cameras;
        [SerializeField] private float attackDistance = 1;
        [SerializeField] private LayerMask attackMask;
        [field: SerializeField] public int Money { get; private set; }
        [field: SerializeField] public int Xp { get; private set; }
        [field: SerializeField] public int XpForNextLevel { get; private set; } = 999;

        private Jumper _jumper;
        private EntityCanBeAttacked _entityCanBeAttacked;

        public readonly Inventory Inventory = new(23);

        public readonly EquipmentInventory EquipmentInventory = new(new[]
        {
            EquipmentType.Helmet,
            EquipmentType.Shield,
            EquipmentType.Weapon,
            EquipmentType.Armor
        });


        public override void Awake()
        {
            CurrentPlayer = this;

            base.Awake();
            DirectionalMover = new VelocityMover(Rigidbody, _directionMovementData);
            _jumper = new Jumper(Rigidbody, _jumpData);
            _entityCanBeAttacked = GetComponent<EntityCanBeAttacked>();

            if(PlayerPrefs.HasKey("money")) Money = PlayerPrefs.GetInt("money");

            if(PlayerPrefs.HasKey("Hp") &&  PlayerPrefs.GetFloat("Hp") != 0)
                _entityCanBeAttacked.Health = PlayerPrefs.GetFloat("Hp");
        }

        private void Start()
        {
            _entityCanBeAttacked.OnDeath += OnDeath;
        }

        private void OnDestroy()
        {
            _entityCanBeAttacked.OnDeath -= OnDeath;
        }

        private void Update()
        {
            if (_jumper.IsJumping)
                _jumper.UpdateJump();

            UpdateAnimations();
            UpdateCameras();
        }

        private void FixedUpdate()
        {
            UpdateArmor();
        }


        protected override void UpdateAnimations()
        {
            base.UpdateAnimations();
            animator.SetAnimationState(AnimationType.Jump, _jumper.IsJumping);
        }

        public override void MoveVertically(float verticalDirection)
        {
            if (_jumper.IsJumping)
                return;

            base.MoveVertically(verticalDirection);
        }


        public void StartAttack()
        {
            if (!(animator.SetAnimationState(AnimationType.Attack, true)))
                return;

            animator.ActionRequested += Attack;
            animator.AnimationEnded += EndAttack;
        }

        public void Jump() => _jumper.Jump();

        private void Attack()
        {
            var direction = new Vector2(DirectionalMover.Direction == Direction.Right ? 1 : -1, 0);
            var shift = direction * attackDistance * 2;
            var center = transform.position.ConvertTo<Vector2>() + shift;

            var objects = Physics2D.OverlapCircleAll(center, attackDistance, attackMask);
            if (objects.Length <= 0) return;

            var damage = 1F;
            var weapon = EquipmentInventory.GetItemByType(EquipmentType.Weapon).Item;
            if (weapon != null && weapon is WeaponItem item)
            {
                damage = item.AttackAmount;
            }

            foreach (var c in objects)
            {
                if (c.gameObject.TryGetComponent<EntityCanBeAttacked>(out var withHealth))
                {
                    withHealth.OnAttack(gameObject, damage);
                }
            }
        }

        private void EndAttack()
        {
            animator.ActionRequested -= Attack;
            animator.AnimationEnded -= EndAttack;
            animator.SetAnimationState(AnimationType.Attack, false);
        }

        private void UpdateCameras()
        {
            foreach (var cameraPair in _cameras.DirectionalCameras)
                cameraPair.Value.enabled = cameraPair.Key == DirectionalMover.Direction;
        }

        private void UpdateArmor()
        {
            var armor = 0F;
            var items = new[]
            {
                EquipmentInventory.GetItemByType(EquipmentType.Armor)?.Item,
                EquipmentInventory.GetItemByType(EquipmentType.Shield)?.Item,
                EquipmentInventory.GetItemByType(EquipmentType.Helmet)?.Item,
            };
            foreach (var item in items)
            {
                if (item is ArmorItem armorItem)
                {
                    armor += armorItem.ArmorAmount;
                }
            }

            _entityCanBeAttacked.Armor = armor;
        }

        public void AddMoney(int money)
        {
            Money += money;
            PlayerPrefs.SetInt("money", Money);
        }

        public void AddXp(int xp)
        {
            Xp += xp;

            if (Xp >= XpForNextLevel) 
            {
                int scene = SceneManager.GetActiveScene().buildIndex;

                if(scene == 1) SceneManager.LoadScene("NextLevel_Scene");
                if(scene == 3) SceneManager.LoadScene("Finish_Scene");
            }

        }

        private void OnDeath()
        {
            PlayerPrefs.DeleteKey("money");
            PlayerPrefs.DeleteKey("Hp");

            SceneManager.LoadScene("Die_scene");
        }
    }
}
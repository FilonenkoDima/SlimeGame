using Core.Animation;
using Core.Movement.Controller;
using NPC.Controller;
using UnityEngine;
using UnityEngine.Rendering;

namespace NPC.Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseEntityBehaviour : MonoBehaviour
    {
        [SerializeField] protected AnimatorController animator;

        protected Rigidbody2D Rigidbody;
        protected DirectionalMover DirectionalMover;
        public Entity EntityLinked;
        private SortingGroup _sortingGroup;

        public virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            _sortingGroup = GetComponent<SortingGroup>();
        }

        public float VerticalPosition => transform.position.y;

        public void SetDrawingOrder(int order) => _sortingGroup.sortingOrder = order;
        public void SetSize(Vector2 size) => transform.localScale = size;
        public void MoveHorizontally(float direction) => DirectionalMover.MoveHorizontally(direction);
        public virtual void MoveVertically(float direction) => DirectionalMover.MoveVertically(direction);

        public void SetVerticalPosition(float verticalPosition) =>
            Rigidbody.position = new Vector2(Rigidbody.position.x, verticalPosition);

        protected virtual void UpdateAnimations()
        {
            animator.SetAnimationState(AnimationType.Idle, true);
            animator.SetAnimationState(AnimationType.Run, DirectionalMover.IsMoving);
        }
    }
}
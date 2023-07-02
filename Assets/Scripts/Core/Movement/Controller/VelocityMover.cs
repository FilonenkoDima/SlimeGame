using Core.Enums;
using Core.Movement.Data;
using UnityEngine;

namespace Core.Movement.Controller
{
    public class VelocityMover : DirectionalMover
    {
        private Vector2 _directionVector;
        private readonly float _vMovementScale;
        public override bool IsMoving => _directionVector.magnitude > 0;
        public override bool IsLeftTurned => true;

        private readonly DirectionalMovementData _directionalMovementData;

        public VelocityMover(Rigidbody2D rigidbody, DirectionalMovementData directionalMovementData) : base(rigidbody)
        {
            _directionalMovementData = directionalMovementData;
            _vMovementScale = directionalMovementData.HorizontalSpeed;
        }

        public override void MoveHorizontally(float direction)
        {
            _directionVector.x = direction;
            Vector2 velocity = Rigidbody.velocity;
            velocity.x = direction * _directionalMovementData.HorizontalSpeed;
            Rigidbody.velocity = velocity;
            if (direction == 0)
                return;

            SetDirection(direction > 0 ? Direction.Right : Direction.Left);
        }

        public override void MoveVertically(float direction)
        {
            _directionVector.y = direction;
            Vector2 velocity = Rigidbody.velocity;
            velocity.y = direction * _directionalMovementData.VerticalSpeed;
            Rigidbody.velocity = velocity;
        }
    }
}
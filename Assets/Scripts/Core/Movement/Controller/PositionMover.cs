using Core.Enums;
using UnityEngine;

namespace Core.Movement.Controller
{
    public class PositionMover: DirectionalMover
    {
        private Vector2 _destination;
        public override bool IsMoving => _destination != Rigidbody.position;
        public override bool IsLeftTurned => false;

        public PositionMover(Rigidbody2D rigidbody) : base(rigidbody) {}

        public override void MoveHorizontally(float horizontalMovement)
        {
            var prev = _destination;
            
            _destination.x = horizontalMovement;
            var newPosition = new Vector2(horizontalMovement, Rigidbody.position.y);
            Rigidbody.MovePosition(newPosition);
            if(_destination.x != Rigidbody.position.x)
                SetDirection(_destination.x > prev.x ? Direction.Right : Direction.Left);
        }

        public override void MoveVertically(float verticalMovement)
        {
            _destination.y = verticalMovement;
            var newPosition = new Vector2(Rigidbody.position.x, verticalMovement);
            Rigidbody.MovePosition(newPosition);
        }
    }
}
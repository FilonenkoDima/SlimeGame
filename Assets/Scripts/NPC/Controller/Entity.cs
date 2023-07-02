using System;
using Drawing;
using NPC.Behaviour;
using UnityEngine;

namespace NPC.Controller
{
    public class Entity : ILevelGraphicElement
    {
        private readonly BaseEntityBehaviour _entityBehaviour;

        public event Action<ILevelGraphicElement> VerticalPositionChanged;

        protected Entity(BaseEntityBehaviour entityBehaviour)
        {
            _entityBehaviour = entityBehaviour;
            _entityBehaviour.EntityLinked = this;
        }

        public float VerticalPosition => _entityBehaviour.VerticalPosition;
        public void SetDrawingOrder(int order) => _entityBehaviour.SetDrawingOrder(order);
        public void SetSize(Vector2 size) => _entityBehaviour.SetSize(size);

        public void SetVerticalPosition(float verticalPosition) =>
            _entityBehaviour.SetVerticalPosition(verticalPosition);

        public void OnVerticalPositionChanged() => VerticalPositionChanged?.Invoke(this);
    }
}
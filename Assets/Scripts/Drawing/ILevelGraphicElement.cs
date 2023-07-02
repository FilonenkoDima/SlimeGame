using System;
using UnityEngine;

namespace Drawing
{
    public interface ILevelGraphicElement 
    {
        float VerticalPosition {get;}
        event Action<ILevelGraphicElement> VerticalPositionChanged;
        void SetDrawingOrder(int order);
        void SetSize(Vector2 size);
        void SetVerticalPosition(float verticalPosition);
    }
}

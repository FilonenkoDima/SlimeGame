using UnityEngine;

namespace UI.Core
{
    [RequireComponent(typeof(Canvas))]
    public abstract class ScreenView : MonoBehaviour
    {
        protected Canvas Canvas;

        protected virtual void Awake()
        {
            Canvas = GetComponent<Canvas>();
        }

        public void Show() => Canvas.enabled = true;
        public void Hide() => Canvas.enabled = false;
    }
}
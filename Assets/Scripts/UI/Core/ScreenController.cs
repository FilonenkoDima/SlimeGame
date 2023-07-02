using UI.Enum;

namespace UI.Core
{
    public abstract class ScreenController<TView> : IScreenController where TView : ScreenView
    {
        protected readonly TView View;
        protected readonly UIContext UIContext;

        protected ScreenController(UIContext uiContext, TView view)
        {
            UIContext = uiContext;
            View = view;
        }

        public virtual void OnOpenRequest() => View.Show();
        public virtual void OnCloseRequest() => View.Hide();

        public abstract void OnInit();
        public abstract ScreenType GetScreenType();
    }
}
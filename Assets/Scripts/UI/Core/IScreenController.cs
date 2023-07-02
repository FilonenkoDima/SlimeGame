using UI.Enum;

namespace UI.Core
{
    public interface IScreenController
    {
        void OnInit();
        void OnOpenRequest();
        void OnCloseRequest();
        ScreenType GetScreenType();
    }
}
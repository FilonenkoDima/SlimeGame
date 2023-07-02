using UnityEngine;
using UnityEngine.UI;

namespace InputReader
{
    public class GameUIInputView : MonoBehaviour, IEntityInputSource
    {
        [SerializeField] private Joystick joystick;
        [SerializeField] private Button jumpButton;
        [SerializeField] private Button attackButton;
        [SerializeField] private Button inventoryButton;

        public float HorizontalDirection => joystick.Horizontal;
        public float VerticalDirection => joystick.Vertical;

        public bool Jump { get; private set; }
        public bool Attack { get; private set; }
        public bool InventoryClicked { get; private set; }

        private void Awake()
        {
            jumpButton.onClick.AddListener(() => Jump = true);
            attackButton.onClick.AddListener(() => Attack = true);
            inventoryButton.onClick.AddListener(() => InventoryClicked = true);
        }

        private void OnDestroy()
        {
            jumpButton.onClick.RemoveAllListeners();
            attackButton.onClick.RemoveAllListeners();
        }

        public void ResetOneTimeActions()
        {
            Jump = false;
            Attack = false;
            InventoryClicked = false;
        }
    }
}
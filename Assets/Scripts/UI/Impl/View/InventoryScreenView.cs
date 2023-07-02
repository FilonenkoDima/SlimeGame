using System;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Impl.View
{
    public class InventoryScreenView : ScreenView
    {
        [field: SerializeField] public GameObject BackpackSlotsHolder { private set; get; }
        [field: SerializeField] public GameObject EquipmentSlotsHolder { private set; get; }
        [field: SerializeField] public Button CloseBtn { private set; get; }
        [field: SerializeField] public MovingImage MovingImage { private set; get; }

        public event Action OnCloseButton;

        protected override void Awake()
        {
            base.Awake();
            CloseBtn.onClick.AddListener(OnCloseButtonInternal);
        }

        private void OnCloseButtonInternal()
        {
            OnCloseButton?.Invoke();
        }
    }
}
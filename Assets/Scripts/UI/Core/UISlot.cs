using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ui.Core
{
    public class UISlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        // [field: SerializeField] public int Index { get; private set; }

        [SerializeField] private Sprite emptyBackgroundSprite;
        [SerializeField] private Sprite emptyItemSprite;
        [SerializeField] private Color emptyItemColor = Color.white;
        [SerializeField] private Image itemImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TMP_Text itemAmount;
        private Button _slotButton;

        public event Action<PointerEventData> SlotDragStart;
        public event Action<PointerEventData> SlotDragging;
        public event Action<PointerEventData> SlotDragEnd;


        public void SetItem(Sprite iconSprite, Sprite itemBackSprite, int amount)
        {
            if (itemBackSprite == null)
            {
                itemBackSprite = emptyBackgroundSprite;
            }

            backgroundImage.sprite = itemBackSprite;

            itemImage.sprite = iconSprite;
            itemImage.color = Color.white;

            itemAmount.text = amount > 1 ? amount.ToString() : "";
        }

        public void ClearItem()
        {
            backgroundImage.sprite = emptyBackgroundSprite;
            itemImage.sprite = emptyItemSprite;
            itemImage.color = emptyItemColor;

            itemAmount.text = "";
        }

        private void Awake()
        {
            _slotButton = GetComponent<Button>();
            ClearItem();
        }

        public void OnBeginDrag(PointerEventData eventData) => SlotDragStart?.Invoke(eventData);
        public void OnDrag(PointerEventData eventData) => SlotDragging?.Invoke(eventData);
        public void OnEndDrag(PointerEventData eventData) => SlotDragEnd?.Invoke(eventData);
    }
}
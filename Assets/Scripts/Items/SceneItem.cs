using System;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;

namespace Items
{
    public class SceneItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TMP_Text tMPText;
        [SerializeField] private Button button;
        [SerializeField] private Canvas canvas;
        [SerializeField] private LayerMask mask;
        [SerializeField] private float interactionDistance;

        [Header("SizeControl")] [SerializeField]
        private float minSize;

        [SerializeField] private float maxSize;
        [SerializeField] private float minVerticalPosition;
        [SerializeField] private float maxVerticalPosition;
        [SerializeField] private Transform itemTransform;

        [SerializeField] private float dropAnimDuration;
        [SerializeField] private float dropRotation;
        [SerializeField] private float dropRadius;

        private Sequence _sequence;
        private float _sizeModificator;

        private event Action<SceneItem> ItemClicked;

        private void Awake()
        {
            button.onClick.AddListener(() => ItemClicked?.Invoke(this));
            var positionDifference = maxVerticalPosition - minVerticalPosition;
            var sizeDifference = maxSize - minSize;
            _sizeModificator = sizeDifference / positionDifference;
        }

        private void OnMouseDown() => ItemClicked?.Invoke(this);

        public void SetItem(Sprite texture, string text, Color textColor)
        {
            spriteRenderer.sprite = texture;
            tMPText.text = text;
            tMPText.color = textColor;
            canvas.enabled = false;
        }

        private void UpdateSize()
        {
            var verticalDelta = maxVerticalPosition - itemTransform.position.y;
            var currentSizeModificator = minSize + _sizeModificator * verticalDelta;
            itemTransform.localScale = Vector2.one * currentSizeModificator;
        }

        public void ActualizeItemData(Sprite texture, string text, Color textColor)
        {
            if (spriteRenderer.sprite != texture)
            {
                spriteRenderer.sprite = texture;
            }
            else if (tMPText.text != text)
            {
                tMPText.text = text;
            }
            else if (tMPText.color != textColor)
            {
                tMPText.color = textColor;
            }
        }

        public void PlayDropAnimation(Vector2 targetPos)
        {
            transform.position = targetPos;
            Vector2 movePosition = transform.position + new Vector3(Random.Range(-dropRadius, dropRadius), 0, 0);
            _sequence = DOTween.Sequence();
            _sequence.Join(transform.DOMove(movePosition, dropAnimDuration));
            _sequence.Join(itemTransform.DORotate
                (new Vector3(0, 0, Random.Range(-dropRotation, dropRotation)), dropAnimDuration));
            _sequence.OnComplete(() =>
            {
                if (!gameObject.IsDestroyed())
                {
                    canvas.enabled = true;
                }
            });
        }

        public void RegisterPickupAction(Action<SceneItem> action)
        {
            ItemClicked += action;
        }

        public void UnregisterPickupAction(Action<SceneItem> action)
        {
            ItemClicked -= action;
        }

        public bool CanBePickedUp()
        {
            var player = Physics2D.OverlapCircle(transform.position, interactionDistance, mask);
            return player != null;
        }
    }
}
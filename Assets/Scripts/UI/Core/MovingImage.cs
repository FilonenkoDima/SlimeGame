using UnityEngine;
using UnityEngine.UI;

namespace UI.Core
{
    public class MovingImage : MonoBehaviour
    {
        [SerializeField] private Image itemImage;

        public void SetItemImage(Sprite image)
        {
            itemImage.sprite = image;
            itemImage.gameObject.SetActive(image != null);
        }
    }
}
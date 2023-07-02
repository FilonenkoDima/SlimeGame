using UnityEngine;

namespace NPC.Behaviour
{
    public class EntityHpBar : MonoBehaviour
    {
        private float _startScaleX;

        private void Start()
        {
            _startScaleX = transform.localScale.x;
        }

        public void UpdateHp(float health, float maxHealth)
        {
            var percents = health / maxHealth;
            var scaleX = _startScaleX * percents;
            var localScale = transform.localScale;
            
            transform.localScale = new Vector3(scaleX, localScale.y, localScale.z);
        }
    }
}
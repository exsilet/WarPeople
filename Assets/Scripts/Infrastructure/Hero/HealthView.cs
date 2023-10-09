using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Hero
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Image[] _emptyIcons;
        [SerializeField] private Sprite _fillIcon;
        [SerializeField] private Sprite _emptyIcon;

        public void IconHealth()
        {
            foreach (Image iconHealth in _emptyIcons)
                iconHealth.overrideSprite = _fillIcon;
        }

        public void DrawingLives(int countHealth)
        {
            for (int i = 0; i < _emptyIcons.Length; i++)
            {
                if (i < countHealth)
                    _emptyIcons[i].overrideSprite = _fillIcon;
                else
                    _emptyIcons[i].overrideSprite = _emptyIcon;
            }
        }
    }
}
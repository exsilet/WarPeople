using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Hero
{
    public class HeroHealthView : MonoBehaviour
    {
        [SerializeField] private Image[] _emptyIcons;
        [SerializeField] private Sprite _fillIcon;
        [SerializeField] private Sprite _emptyIcon;
        [SerializeField] private Fighter _fighter;
        
        private HeroHealth _heroHealth;

        private void Start()
        {
            _heroHealth = _fighter.gameObject.GetComponent<HeroHealth>();
            _heroHealth.HealthChanged += ShowApplyDamage;
            //IconHealth();
        }

        private void OnDisable()
        {
            _heroHealth.HealthChanged -= ShowApplyDamage;
        }
        
        public void IconHealth()
        {
            foreach (Image iconHealth in _emptyIcons)
                iconHealth.overrideSprite = _fillIcon;
        }

        public void ShowApplyDamage(int currentHp)
        {
            ApplyDamage(currentHp);
        }

        private void ApplyDamage(int countHealth)
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
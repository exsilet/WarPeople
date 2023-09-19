using Assets.Scripts.Fighters;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    public class ChooseFighter : MonoBehaviour
    {
        [SerializeField] private Image _fighterImage;
        [SerializeField] private Button _nextFighter;
        [SerializeField] private Button _previousFighter;
        [SerializeField] private List<Fighter> _fighters;
        [SerializeField] private SkillPanelUI _panelUI;

        private int _currentFighterIndex;
        private Fighter _currentFighter;

        public Fighter CurrentFighter => _currentFighter;

        private void Start()
        {
            _fighterImage.sprite = _fighters[0].FighterData.FighterIcon;
            _currentFighter = _fighters[0];
        }

        private void OnEnable()
        {
            _nextFighter.onClick.AddListener(ChooseNext);
            _previousFighter.onClick.AddListener(ChoosePrevious);
        }

        private void OnDisable()
        {
            _nextFighter.onClick.RemoveListener(ChooseNext);
            _previousFighter.onClick.RemoveListener(ChoosePrevious);
        }

        private void ChooseNext()
        {
            _currentFighterIndex++;

            if (_currentFighterIndex > _fighters.Count - 1)
            {
                _currentFighterIndex = 0;                
            }
            _currentFighter = _fighters[_currentFighterIndex];
            _panelUI.ShowSkills();
            _fighterImage.sprite = _fighters[_currentFighterIndex].FighterData.FighterIcon;
        }

        private void ChoosePrevious()
        {
            _currentFighterIndex--;

            if (_currentFighterIndex < 0)
            {
                _currentFighterIndex = _fighters.Count - 1;
            }
            _currentFighter = _fighters[_currentFighterIndex];
            _panelUI.ShowSkills();
            _fighterImage.sprite = _fighters[_currentFighterIndex].FighterData.FighterIcon;
        }
    }
}

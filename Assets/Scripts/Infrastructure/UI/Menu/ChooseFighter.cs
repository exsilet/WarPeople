using MultiPlayer;
using StaticData;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Infrastructure.UI.Menu
{
    public class ChooseFighter : MonoBehaviour
    {
        [SerializeField] private Image _fighterImage;
        [SerializeField] private Button _nextFighter;
        [SerializeField] private Button _previousFighter;
        [SerializeField] private Button _selectFighter;
        [SerializeField] private List<PlayerStaticData> _fighters;
        [SerializeField] private SkillPanelUI _panelUI;
        [SerializeField] private StartServer _startServer;

        private int _currentFighterIndex;
        private PlayerStaticData _currentFighter;

        public event UnityAction<PlayerStaticData> PlayerChanged;

        public PlayerStaticData CurrentFighter => _currentFighter;        

        private void Awake()
        {
            _fighterImage.sprite = _fighters[0].Icon;
            _currentFighter = _fighters[0];
        }        

        private void OnEnable()
        {
            _nextFighter.onClick.AddListener(ChooseNext);
            _previousFighter.onClick.AddListener(ChoosePrevious);
            PlayerChanged += _startServer.SetPlayerData;
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
            _fighterImage.sprite = _fighters[_currentFighterIndex].Icon;
            PlayerChanged?.Invoke(_currentFighter);
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
            _fighterImage.sprite = _fighters[_currentFighterIndex].Icon;
            PlayerChanged?.Invoke(_currentFighter);
        }
    }
}

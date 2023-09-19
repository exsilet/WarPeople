using Assets.Scripts.Fighters;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Menu
{
    public class SkillPanelUI : MonoBehaviour
    {
        [SerializeField] private List<SkillView> _skillViewsPrefabs;
        [SerializeField] private ChooseFighter _chooseFighter;

        private List<SkillView> _skillViews = new();
        private bool _isStarted;
        private SkillView _skillView;

        private void Start()
        {
            ShowSkills();
        }
        
        public void ShowSkills()
        {
            for (int i = 0; i < _chooseFighter.CurrentFighter.FighterData.SkillDatas.Count; i++)
            {
                _skillViewsPrefabs[i].Initialize(_chooseFighter.CurrentFighter.FighterData.SkillDatas[i]);
            }
        }
    }
}

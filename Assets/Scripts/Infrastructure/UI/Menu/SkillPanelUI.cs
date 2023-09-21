using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.UI.Menu
{
    public class SkillPanelUI : MonoBehaviour
    {
        [SerializeField] private List<SkillView> _skillViewsPrefabs;
        [SerializeField] private ChooseFighter _chooseFighter;

        private List<SkillView> _skillViews = new();
        private SkillView _skillView;

        private void Start()
        {
            ShowSkills();
        }

        public void ShowSkills()
        {
            for (int i = 0; i < _chooseFighter.CurrentFighter.SkillDatas.Count; i++)
            {
                _skillViewsPrefabs[i].Initialize(_chooseFighter.CurrentFighter.SkillDatas[i]);
            }
        }
    }
}

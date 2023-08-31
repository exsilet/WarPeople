using System.Collections.Generic;
using UnityEngine;

public class SkillsPanel : MonoBehaviour
{
    [SerializeField] private List<SkillStaticData> _skills;
    [SerializeField] private List<SkillView> _skillViewsPrefabs;
    [SerializeField] private Inventory _inventory;

    private List<SkillView> _skillViews = new();
    private bool _isStarted;
    private SkillView _skillView;

    private void Start()
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            _skillViews.Add(_skillViewsPrefabs[i]);
            _skillViewsPrefabs[i].Initialize(_skills[i]);
        }

        _isStarted = true;
        OnEnable();
    }

    private void OnEnable()
    {
        if (_isStarted == false)
            return;

        foreach (SkillView view in _skillViews)
        {
            view.AddSkillsButton += AddSkills;
        }
    }

    private void OnDisable()
    {
        foreach (SkillView view in _skillViews)
        {
            view.AddSkillsButton -= AddSkills;
        }
    }

    private void AddSkills(SkillStaticData data, SkillView view)
    {
        if (view.CurrentCount > 0)
        {
            _inventory.BySkills(data);

            if (data.Count > 0)
            {
                view.CountSkill();
            }
        }
    }

    public void AddCountSkill(SkillViewAttack view, SkillStaticData data)
    {
        foreach (SkillView dataView in _skillViews)
        {
            if(dataView.SkillStaticData == data)
                dataView.AddCurrentCount();
        }
    }
}
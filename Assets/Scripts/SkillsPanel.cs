using System.Collections.Generic;
using StaticData;
using UnityEngine;

public class SkillsPanel : MonoBehaviour
{
    [SerializeField] private List<SkillView> _skillViewsPrefabs;
    [SerializeField] private Inventory _inventory;

    public List<SkillView> _skillViews = new();
    private List<SkillStaticData> _skills = new();
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

    public void AddPlayerSkills(SkillStaticData staticData)
    {
        _skills.Add(staticData);
    }

    public void NoActivePanel()
    {
        foreach (SkillView view in _skillViewsPrefabs)
            view.BattleInteractive();
    }

    public void ActivePanel()
    {
        foreach (SkillView view in _skillViewsPrefabs)
            view.StopBattle();
    }

    public void AddCountSkill(SkillViewAttack view, SkillStaticData data)
    {
        foreach (SkillView dataView in _skillViews)
            if (dataView.SkillStaticData == data)
                dataView.AddCurrentCount();
    }

    private void OnEnable()
    {
        if (_isStarted == false)
            return;

        foreach (SkillView view in _skillViews)
            view.AddSkillsButton += AddSkills;
    }

    private void OnDisable()
    {
        foreach (SkillView view in _skillViews)
            view.AddSkillsButton -= AddSkills;
    }

    private void AddSkills(SkillStaticData data, SkillView view)
    {
        if (view.CurrentCount > 0)
        {
            _inventory.BySkills(data);
            if (data.Count > 0)
                view.CountSkill();
        }
    }
}
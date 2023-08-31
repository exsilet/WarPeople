using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private SkillsPanel _skillsPanel;
    [SerializeField] private Image _icon;
    [SerializeField] private List<SkillViewAttack> _skillViewsAttackPrefabs;

    private int _maxCount = 10;
    private int _countSkill;
    private bool _isStarted;
    private List<SkillStaticData> _skillsList;
    public List<SkillViewAttack> _skillViewAttack = new();

    private void Start()
    {
        _skillsList = new List<SkillStaticData>();

        _isStarted = true;
        OnEnable();
    }

    public void BySkills(SkillStaticData staticData)
    {
        for (int i = 0; i < _skillViewsAttackPrefabs.Count; i++)
        {
            if (_skillViewsAttackPrefabs[i].Initialized == false)
            {
                _skillsList.Insert(i, staticData);
                _skillViewsAttackPrefabs[i].SetInitialized();
                AddAttackSkill(i);
                break;
            }
        }
    }

    private void AddAttackSkill(int countItem)
    {
        if (_skillViewsAttackPrefabs.Count < _maxCount)
            return;

        if (_countSkill < _skillsList.Count)
        {
            if (_skillViewsAttackPrefabs[countItem].Initialized == true)
            {
                _skillViewAttack.Insert(countItem, _skillViewsAttackPrefabs[countItem]);
                _skillViewsAttackPrefabs[countItem].Initialize(_skillsList[countItem]);
                Debug.Log("update skill");
            }
        }
    }

    private void OnEnable()
    {
        if (_isStarted == false)
            return;

        foreach (SkillViewAttack view in _skillViewsAttackPrefabs)
        {
            view.RemoveSkillsButton += RemoveSkills;
        }
    }

    private void OnDisable()
    {
        foreach (SkillViewAttack view in _skillViewsAttackPrefabs)
        {
            view.RemoveSkillsButton -= RemoveSkills;
        }
    }

    private void RemoveSkills(SkillStaticData data, SkillViewAttack view)
    {
        view.RemoveSkill();
        _skillsPanel.AddCountSkill(view, data);
        _skillsList.Remove(data);
        _skillViewAttack.Remove(view);
    }

    public void RemoveWarPlayer(SkillStaticData data, SkillViewAttack view)
    {
        _skillsPanel.AddCountSkill(view, data);
        _skillsList.Remove(data);
        _skillViewAttack.Remove(view);
    }
}
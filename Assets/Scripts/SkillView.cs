using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillView : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _addSkill;
    [SerializeField] private TMP_Text _countText;

    private int _currentCount;
    private SkillStaticData _skillStaticData;
    private bool _isInitialized;
    public int CurrentCount => _currentCount;
    public SkillStaticData SkillStaticData => _skillStaticData;
    public event UnityAction<SkillStaticData, SkillView> AddSkillsButton;

    private void Update()
    {
        //InteractiveSkill();
    }

    public void CountSkill()
    {
        if (_currentCount == 0)
            return;
        
        _currentCount -= 1;
        _countText.text = _currentCount.ToString();
        
        if (_currentCount == 0)
            BattleInteractive();
    }

    public void Initialize(SkillStaticData skillStaticData)
    {
        _skillStaticData = skillStaticData;
        _icon.sprite = skillStaticData.UIIcon;
        _label.text = skillStaticData.Label;
        _description.text = skillStaticData.Description;
        _skillStaticData.CountLimit = skillStaticData.CountLimit;
        _currentCount = skillStaticData.Count;
        _countText.text = _currentCount.ToString();
        
        _isInitialized = true;
        OnEnable();
    }

    public void AddCurrentCount()
    {
        if (_currentCount < _skillStaticData.CountLimit)
        {
            _currentCount += 1;
            _countText.text = _currentCount.ToString();
        }
    }
    
    public void BattleInteractive() => _addSkill.interactable = false;
    public void StopBattle() => _addSkill.interactable = true;

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _addSkill.onClick.AddListener(OnClick);
    }

    private void OnDisable() => _addSkill.onClick.RemoveListener(OnClick);
    private void OnClick() => AddSkillsButton?.Invoke(_skillStaticData, this);
}
﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillViewAttack : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Button _removeSkill;

    private bool _isInitialized;
    public bool Initialized => _isInitialized;
    private SkillStaticData _skillStaticData;
    public event UnityAction<SkillStaticData, SkillViewAttack> RemoveSkillsButton;
    public SkillStaticData SkillStaticData => _skillStaticData;

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _removeSkill.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _removeSkill.onClick.RemoveListener(OnClick);
    }

    public void Initialize(SkillStaticData skillStaticData)
    {
        _skillStaticData = skillStaticData;
        _icon.sprite = skillStaticData.UIIcon;
        
        _isInitialized = true;
        OnEnable();
    }

    public void RemoveSkill()
    {
        _icon.sprite = null;
        _isInitialized = false;
        _skillStaticData = null;
    }

    public void SetInitialized()
    {
        _isInitialized = !_isInitialized;
    }

    private void OnClick()
    {
        RemoveSkillsButton?.Invoke(_skillStaticData, this);
    }
}
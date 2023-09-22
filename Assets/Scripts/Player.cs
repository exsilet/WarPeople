using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure.Hero;
using StaticData;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _stopSecond;
    [SerializeField] private float _hidePlayed;
    [SerializeField] private PlayerAnimator _animator;

    private PlayerStaticData _playerData;
    private Inventory _inventory;
    private SkillsPanel _skillsPanel;

    private bool _isInitialized;
    private List<SkillViewAttack> _viewAttacks = new();

    public void Construct(SkillsPanel skillsPanel, Inventory inventory)
    {
        _skillsPanel = skillsPanel;
        _inventory = inventory;
    }

    public void SetPlayerData(PlayerStaticData staticData)
        => _playerData = staticData;

    public void AttackSkill()
    {
        foreach (SkillViewAttack data in _inventory._skillViewAttack)
        {
            _viewAttacks.Add(data);
        }
        
        _skillsPanel.NoActivePanel();
        _animator.PlayStand();
        
        AttackPlayer();
    }

    private void AttackPlayer()
    {
        foreach (SkillViewAttack data in _inventory._skillViewAttack)
        {
            data.Show();
        }
        
        StartCoroutine(PlaySkill());
    }

    private IEnumerator PlaySkill()
    {
        foreach (SkillViewAttack data in _inventory._skillViewAttack)
        {
            yield return new WaitForSeconds(_stopSecond);
            ChoiceAttack(data);
            data.Hide();
            yield return new WaitForSeconds(_hidePlayed);
            data.RemoveAttack();
            Debug.Log("war");
        }

        OnEnd();
    }

    private void OnEnd()
    {
        StopCoroutine(PlaySkill());
        _animator.PlayStopAnimation();
        _inventory.RemoveWarPlayer();
        _viewAttacks.Clear();
        _skillsPanel.ActivePanel();
    }

    
    private void ChoiceAttack(SkillViewAttack data)
    {
        switch (data.SkillStaticData.Type)
        {
            case SkillTypeId.Attack:
                _animator.PlayHit();
                break;
            case SkillTypeId.Defence:
                _animator.PlayDefenceAnimation();
                break;
            case SkillTypeId.Evasion:
                _animator.PlayEvasionAnimation();
                break;
            case SkillTypeId.SuperAttack:
                _animator.PlaySuperAttackAnimation();
                break;
            case SkillTypeId.Counterstrike:
                _animator.PlayCounterstrikeAnimation();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
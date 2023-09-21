using System;
using System.Collections;
using System.Collections.Generic;
using StaticData;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _stopSecond;
    [SerializeField] private float _hidePlayed;

    private PlayerStaticData _playerData;
    private Inventory _inventory;
    private SkillsPanel _skillsPanel;

    private const string Attack = "Attack";
    private const string Stand = "Stand";
    private const string Protection = "Protection";
    private const string Dodge = "Dodge";
    private const string StrongAttack = "StrongAttack";
    private const string Recharge = "Recharge";
    private const string StopAnimation = "StopAnimation";
    private Animator _animator;
    private bool _isInitialized;
    private List<SkillViewAttack> _viewAttacks = new();

    public void Construct(SkillsPanel skillsPanel, Inventory inventory)
    {
        _skillsPanel = skillsPanel;
        _inventory = inventory;
    }
    
    private void Start()
    {        
        _animator = GetComponent<Animator>();
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
        _animator.SetTrigger(Stand);
        
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
        _animator.SetTrigger(StopAnimation);
        _inventory.RemoveWarPlayer();
        _viewAttacks.Clear();
        _skillsPanel.ActivePanel();
    }

    private void Hit() => _animator.SetTrigger(Attack);

    private void DefenceAnimation() => _animator.SetTrigger(Protection);

    private void EvasionAnimation() => _animator.SetTrigger(Dodge);

    private void SuperAttackAnimation() => _animator.SetTrigger(StrongAttack);

    private void CounterstrikeAnimation() => _animator.SetTrigger(Recharge);

    private void ChoiceAttack(SkillViewAttack data)
    {
        switch (data.SkillStaticData.Type)
        {
            case SkillTypeId.Attack:
                Hit();
                break;
            case SkillTypeId.Defence:
                DefenceAnimation();
                break;
            case SkillTypeId.Evasion:
                EvasionAnimation();
                break;
            case SkillTypeId.SuperAttack:
                SuperAttackAnimation();
                break;
            case SkillTypeId.Counterstrike:
                CounterstrikeAnimation();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
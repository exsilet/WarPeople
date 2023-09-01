using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Button _battle;
    [SerializeField] private float _stopSecond;
    [SerializeField] private float _hidePlayed;

    private const string Attack = "Attack";
    private const string Stand = "Stand";
    private const string Protection = "Protection";
    private const string Dodge = "Dodge";
    private const string StrongAttack = "StrongAttack";
    private const string Recharge = "Recharge";
    private Animator _animator;
    private bool _isInitialized;
    private List<SkillViewAttack> _viewAttacks = new();

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _battle.onClick.AddListener(AttackSkill);
    }

    private void OnDisable() =>
        _battle.onClick.RemoveListener(AttackSkill);

    private void AttackSkill()
    {
        foreach (SkillViewAttack data in _inventory._skillViewAttack)
        {
            _viewAttacks.Add(data);
        }

        _animator.SetTrigger(Stand);
        
        AttackPlayer();
    }

    private void Hit() => _animator.SetTrigger(Attack);
    private void DefenceAnimation() => _animator.SetTrigger(Protection);
    private void EvasionAnimation() => _animator.SetTrigger(Dodge);
    private void SuperAttackAnimation() => _animator.SetTrigger(StrongAttack);
    private void CounterstrikeAnimation() => _animator.SetTrigger(Recharge);

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
        
    }

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
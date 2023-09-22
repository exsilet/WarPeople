﻿using System;
using Logic;
using UnityEngine;

namespace Infrastructure.Hero
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour, IAnimationStateReader
    {
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int StandHash = Animator.StringToHash("Stand");
        private static readonly int ProtectionHash = Animator.StringToHash("Protection");
        private static readonly int DodgeHash = Animator.StringToHash("Dodge");
        private static readonly int StrongAttackHash = Animator.StringToHash("StrongAttack");
        private static readonly int RechargeHash = Animator.StringToHash("Recharge");
        private static readonly int DieHash = Animator.StringToHash("Died");
        private static readonly int StopAnimation = Animator.StringToHash("StopAnimation");
        
        private readonly int _deathStateHash = Animator.StringToHash("Died");
        private readonly int _attackStateHash = Animator.StringToHash("attack");
        private readonly int _standStateHash = Animator.StringToHash("Stand");
        private readonly int _protectionStateHash = Animator.StringToHash("Protection");
        private readonly int _dodgeStateHash = Animator.StringToHash("Dodge");
        private readonly int _strongAttackStateHash = Animator.StringToHash("StrongAttack");
        private readonly int _rechargeStateHash = Animator.StringToHash("Recharge");

        private Animator _animator;

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;
        public AnimatorState State { get; private set; }

        private void Awake() =>
            _animator = GetComponent<Animator>();

        public void PlayStand() => _animator.SetTrigger(StandHash);
        public void PlayHit() => _animator.SetTrigger(AttackHash);
        public void PlayDefenceAnimation() => _animator.SetTrigger(ProtectionHash);
        public void PlayEvasionAnimation() => _animator.SetTrigger(DodgeHash);
        public void PlaySuperAttackAnimation() => _animator.SetTrigger(StrongAttackHash);
        public void PlayCounterstrikeAnimation() => _animator.SetTrigger(RechargeHash);
        public void PlayDeath() => _animator.SetTrigger(DieHash);
        public void PlayStopAnimation() => _animator.SetTrigger(StopAnimation);

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash)
        {
            StateExited?.Invoke(StateFor(stateHash));
        }

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == _standStateHash)
            {
                state = AnimatorState.Stand;
            }
            else if (stateHash == _attackStateHash)
            {
                state = AnimatorState.Attack;
            }
            else if (stateHash == _protectionStateHash)
            {
                state = AnimatorState.Protection;
            }
            else if (stateHash == _dodgeStateHash)
            {
                state = AnimatorState.Dodge;
            }
            else if (stateHash == _strongAttackStateHash)
            {
                state = AnimatorState.StrongAttack;
            }
            else if (stateHash == _rechargeStateHash)
            {
                state = AnimatorState.Recharge;
            }
            else if (stateHash == _deathStateHash)
            {
                state = AnimatorState.Died;
            }
            else
            {
                state = AnimatorState.Unknown;
            }

            return state;
        }
    }
}
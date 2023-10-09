using System;
using Logic;
using Photon.Pun;
using UnityEngine;

namespace Infrastructure.Hero
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PhotonAnimatorView))]
    public class PlayerAnimator : MonoBehaviour, IAnimationStateReader
    {
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int ProtectionHash = Animator.StringToHash("Protection");
        private static readonly int DodgeHash = Animator.StringToHash("Dodge");
        private static readonly int StrongAttackHash = Animator.StringToHash("StrongAttack");
        private static readonly int RechargeHash = Animator.StringToHash("Recharge");
        private static readonly int DieHash = Animator.StringToHash("Died");
        
        private readonly int _attackStateHash = Animator.StringToHash("attack");
        private readonly int _protectionStateHash = Animator.StringToHash("Protection");
        private readonly int _dodgeStateHash = Animator.StringToHash("Dodge");
        private readonly int _strongAttackStateHash = Animator.StringToHash("StrongAttack");
        private readonly int _rechargeStateHash = Animator.StringToHash("Recharge");
        private readonly int _deathStateHash = Animator.StringToHash("Died");

        private string _currentState;
        private Animator _animator;
        private PhotonView _photonView;
        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;
        public AnimatorState State { get; private set; }


        private void Awake() =>
            _animator = GetComponent<Animator>();

        private void Start()
            => _photonView = GetComponent<PhotonView>();

        public void PlayAttack()
            => _photonView.RPC(nameof(PlayHit), RpcTarget.All);

        public void PlayDefence()
            => _photonView.RPC(nameof(PlayDefenceAnimation), RpcTarget.All);

        public void PlayCounter()
            => _photonView.RPC(nameof(PlayCounterstrikeAnimation), RpcTarget.All);

        public void PlayEvasion()
            => _photonView.RPC(nameof(PlayEvasionAnimation), RpcTarget.All);

        public void PlaySuperAttack()
            => _photonView.RPC(nameof(PlaySuperAttackAnimation), RpcTarget.All);

        public void PlayDeath()
            => _photonView.RPC(nameof(PlayDeathAnimation), RpcTarget.All);

        [PunRPC]
        private void PlayHit()
        {
            _animator.SetTrigger(AttackHash);
        }
        [PunRPC]
        private void PlayDefenceAnimation() => _animator.SetTrigger(ProtectionHash);
        [PunRPC]
        private void PlayEvasionAnimation() => _animator.SetTrigger(DodgeHash);
        [PunRPC]
        private void PlaySuperAttackAnimation() => _animator.SetTrigger(StrongAttackHash);
        [PunRPC]
        private void PlayCounterstrikeAnimation() => _animator.SetTrigger(RechargeHash);
        [PunRPC]
        private void PlayDeathAnimation() => _animator.SetTrigger(DieHash);

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
            
            if (stateHash == _attackStateHash)
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
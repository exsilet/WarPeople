using System;
using System.Collections;
using MultiPlayer;
using Photon.Pun;
using StaticData;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Hero
{
    [RequireComponent(typeof(PlayerAnimator))]
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PhotonViewComponents))]
    public class Fighter : MonoBehaviour
    {
        [SerializeField] private float _stopSecond;
        [SerializeField] private float _hidePlayed;
        [SerializeField] private PlayerAnimator _animator;

        private PlayerStaticData _playerData;
        private Inventory _inventory;
        private SkillsPanel _skillsPanel;

        private bool _isInitialized;
        private TimerStart _timer;
        private PhotonView _photonView;
        public PlayerStaticData PlayerData => _playerData;
        public PhotonView PhotonView => _photonView;

        public bool _isRoundEnd;
        public event UnityAction<bool> RoundEnded;
        
        private void Start()
        {
            _timer = FindObjectOfType<TimerStart>();
            _photonView = GetComponent<PhotonView>();
        }

        public void Construct(SkillsPanel skillsPanel, Inventory inventory)
        {
            _skillsPanel = skillsPanel;
            _inventory = inventory;
        }

        public void SetPlayerData(PlayerStaticData staticData)
            => _playerData = staticData;

        public void AttackSkill()
        {
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
            
            //_isBattleEnd = false;
            //_timer.AnimationStop();
            _animator.PlayStopAnimation();
            yield return new WaitForSeconds(0.5f);
            OnEndBattle();            
        }

        private void OnEndBattle()
        {
            StopCoroutine(PlaySkill());
            _inventory.RemoveWarPlayer();
            _skillsPanel.ActivePanel();
            _photonView.RPC(nameof(EndBattle), RpcTarget.All);
        }

        [PunRPC]
        public void EndBattle()
        {
            _isRoundEnd = true;
            RoundEnded?.Invoke(_isRoundEnd);
        }

        public void AttackHero()
        {
            
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
}
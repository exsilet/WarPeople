using System;
using System.Collections;
using System.Collections.Generic;
using Logic;
using Photon.Pun;
using StaticData;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Hero
{
    [RequireComponent(typeof(PlayerAnimator))]
    public class Fighter : MonoBehaviour
    {
        [SerializeField] private float _stopSecond;
        [SerializeField] private float _hidePlayed;
        [SerializeField] private PlayerAnimator _animator;

        public PlayerStaticData PlayerData;
        private Inventory _inventory;
        private SkillsPanel _skillsPanel;

        private bool _isInitialized;
        //public List<SkillViewAttack> _viewAttacks = new();
        private TimerStart _timer;
        private PhotonView _photonView;

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

        private void OnEnable()
        {
            
        }        

        public void SetPlayerData(PlayerStaticData staticData)
            => PlayerData = staticData;

        public void AttackSkill()
        {
            // foreach (SkillViewAttack data in _inventory._skillViewAttack)
            // {
            //     _viewAttacks.Add(data);
            // }
        
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
            yield return new WaitForSeconds(0.5f);
            _animator.PlayStopAnimation();
            OnEndBattle();            
        }

        public void OnEndBattle()
        {
            StopCoroutine(PlaySkill());
            _inventory.RemoveWarPlayer();
            //_viewAttacks.Clear();
            _skillsPanel.ActivePanel();
            _photonView.RPC(nameof(EndBattle), RpcTarget.All);
        }

        [PunRPC]
        public void EndBattle()
        {
            _isRoundEnd = true;
            RoundEnded?.Invoke(_isRoundEnd);
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
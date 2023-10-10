using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Infrastructure.Hero;
using Infrastructure.Hero;
using Logic;
using Photon.Pun;
using StaticData;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.EnemyBot
{
    public class BotFighter : MonoBehaviour, IFighter
    {
        [SerializeField] private PlayerAnimator _animator;
        [SerializeField] private float _stopSecond;
        [SerializeField] private float _hidePlayed;
        [SerializeField] private SkillDisplay _skillDisplay;
        
        private PlayerStaticData _playerData;
        private Inventory _inventory;
        private SkillsPanel _skillsPanel;
        private bool _isInitialized;
        private PhotonView _photonView;
        public bool _isRoundEnd;
        private int _countSkill;
        private string _currentSkill;

        public PlayerStaticData PlayerData => _playerData;
        public PhotonView PhotonView => _photonView;
        public event UnityAction<bool> RoundEnded;
        public string CurrentSkill => _currentSkill;

        private void Start()
        {
            _photonView = GetComponent<PhotonView>();

            StartCoroutine(SkillEnemy());
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
            StopCoroutine(SkillEnemy());
            StartCoroutine(PlaySkill());
        }

        private IEnumerator SkillEnemy()
        {
            yield return new WaitForSeconds(1f);
            AddSkillInventory();
            
            //_inventory.RandomSkill();
        }

        private void AddSkillInventory()
        {
            List<SkillStaticData> skillPlayer = _playerData.SkillDatas;
            List<SkillView> viewSkill = _skillsPanel._skillViews;

            for (int i = 0; i < skillPlayer.Count; i++)
            {
                _countSkill += skillPlayer[i].Count;

                while (_countSkill > 0)
                {
                    if (viewSkill[i].CurrentCount > 0)
                    {
                        _inventory.BySkills(skillPlayer[i]);
                    
                        if (skillPlayer[i].Count > 0)
                            viewSkill[i].CountSkill();
                    }
                    else
                        break;
                }
            }
        }

        private IEnumerator PlaySkill()
        {
            foreach (SkillViewAttack data in _inventory._skillViewAttack)
            {
                yield return new WaitForSeconds(_stopSecond);
                ChooseSkill(data);
                data.Hide();
                yield return new WaitForSeconds(_hidePlayed);
                data.RemoveAttack();
            }
            
            yield return new WaitForSeconds(0.5f);
            
            OnEndBattle();            
        }

        private void OnEndBattle()
        {
            StopCoroutine(PlaySkill());
            _inventory.RemoveWarPlayer();
            _skillsPanel.ActivePanel();
            _photonView.RPC(nameof(EndBattle), RpcTarget.All);
            StartCoroutine(SkillEnemy());
        }

        [PunRPC]
        private void EndBattle()
        {
            _isRoundEnd = true;
            RoundEnded?.Invoke(_isRoundEnd);
            _isRoundEnd = false;
        }

        [PunRPC]
        public void SetCurrentSkill(string skill)
            => _currentSkill = skill;

        private void ChooseSkill(SkillViewAttack data)
        {
            switch (data.SkillStaticData.Type)
            {
                case SkillTypeId.Attack:
                    _photonView.RPC(nameof(SetCurrentSkill), RpcTarget.All, SkillTypeId.Attack.ToString());
                    _animator.PlayAttack();
                    _skillDisplay.ShowAttack();
                    break;
                case SkillTypeId.Defence:
                    _photonView.RPC(nameof(SetCurrentSkill), RpcTarget.All, SkillTypeId.Defence.ToString());
                    _animator.PlayDefence();
                    break;
                case SkillTypeId.Evasion:
                    _photonView.RPC(nameof(SetCurrentSkill), RpcTarget.All, SkillTypeId.Evasion.ToString());
                    _animator.PlayEvasion();
                    break;
                case SkillTypeId.SuperAttack:
                    _animator.PlaySuperAttack();
                    break;
                case SkillTypeId.Counterstrike:
                    _photonView.RPC(nameof(SetCurrentSkill), RpcTarget.All, SkillTypeId.Counterstrike.ToString());
                    _animator.PlayCounter();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }  
    }
}
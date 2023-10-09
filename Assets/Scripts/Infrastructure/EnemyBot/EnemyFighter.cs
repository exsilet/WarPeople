using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Infrastructure.Hero;
using Infrastructure.Hero;
using Photon.Pun;
using StaticData;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.EnemyBot
{
    public class EnemyFighter : MonoBehaviour
    {
        [SerializeField] private PlayerAnimator _animator;
        [SerializeField] private float _stopSecond;
        [SerializeField] private float _hidePlayed;
        [SerializeField] private SkillDisplay _skillDisplay;
        
        private PlayerStaticData _playerData;
        private Inventory _inventory;
        private SkillsPanel _skillsPanel;
        private bool _isInitialized;
        private TimerStart _timer;
        private PhotonView _photonView;
        public bool _isRoundEnd;
        private int _countSkill;
        
        public PlayerStaticData PlayerData => _playerData;
        public PhotonView PhotonView => _photonView;
        public event UnityAction<bool> RoundEnded;

        private void Start()
        {
            _timer = FindObjectOfType<TimerStart>();
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
            //int random = Random.Range(0, skillPlayer.Count);
            
            for (int i = 0; i < skillPlayer.Count; i++)
            {
                _countSkill += skillPlayer[i].Count;
                Debug.Log("current skill " + skillPlayer[i].Label);
                Debug.Log("count skills " + _countSkill);

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
                ChoiceAttack(data);
                data.Hide();
                yield return new WaitForSeconds(_hidePlayed);
                data.RemoveAttack();
                Debug.Log("war");
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

        private void ChoiceAttack(SkillViewAttack data)
        {
            switch (data.SkillStaticData.Type)
            {
                case SkillTypeId.Attack:
                    _animator.PlayAttack();
                    _skillDisplay.ShowAttack();
                    break;
                case SkillTypeId.Defence:
                    _animator.PlayDefence();
                    break;
                case SkillTypeId.Evasion:
                    _animator.PlayEvasion();
                    break;
                case SkillTypeId.SuperAttack:
                    _animator.PlaySuperAttack();
                    break;
                case SkillTypeId.Counterstrike:
                    _animator.PlayCounter();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
using System;
using System.Collections;
using Assets.Scripts.Infrastructure.Hero;
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
        [SerializeField] private SkillDisplay _skillDisplay;

        private string _currentSkill;
        public bool _isRoundEnd;
        private bool _isInitialized;
        private PlayerStaticData _playerData;
        private Inventory _inventory;
        private SkillsPanel _skillsPanel;        
        private TimerStart _timer;
        private PhotonView _photonView;

        public PlayerStaticData PlayerData => _playerData;
        public Inventory Inventory => _inventory;
        public PhotonView PhotonView => _photonView;
        public string CurrentSkill => _currentSkill;
        
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
        }

        [PunRPC]
        public void EndBattle()
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
using System.Collections;
using Infrastructure.EnemyBot;
using Infrastructure.Hero;
using Photon.Pun;
using StaticData;
using UnityEngine;

namespace Logic
{
    public class BotEventAnimation : MonoBehaviour
    {
        [SerializeField] private BotFighter _botFighter;
        
        private PhotonView _photonView;
        private PhotonView _photonView2;
        private PlayerStaticData _botData;
        private SkillStaticData _skillData;
        private Health _playerHealth;
        private Health _botHealth;
        private Fighter _player;
        private BotFighter _bot;

        private void Start()
        {
            if(!_botFighter.PhotonView.IsMine)
                return;
            
            _botData = _botFighter.PlayerData;
            _photonView = GetComponent<PhotonView>();

            StartCoroutine(CreateHero());
        }
        
        public void TakeDamage()
        {
            if (_botFighter.CurrentSkill == SkillTypeId.Attack.ToString())
            {
                if (_player.CurrentSkill == SkillTypeId.Defence.ToString())
                {
                    _playerHealth.ApplyDamage(_skillData.Damage / 2);
                }
                else if (_player.CurrentSkill == SkillTypeId.Counterstrike.ToString())
                {
                    _playerHealth.ApplyDamage(_skillData.Damage / 2);
                    _botHealth.ApplyDamage(_skillData.Damage / 2);
                }
                else if (_player.CurrentSkill == SkillTypeId.Evasion.ToString())
                {
                }
                else
                    _playerHealth.ApplyDamage(_skillData.Damage);
            }
        }
        
        private IEnumerator CreateHero()
        {
            yield return new WaitForSeconds(1f);
            TargetBot();
        }

        private void TargetBot()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Hero");

            foreach (GameObject player in players)
            {
                if (player.GetComponent<Fighter>() != null)
                {
                    Debug.Log("target bot ");
                    
                    _bot = _photonView.GetComponent<BotFighter>();
                    _botHealth = _photonView.GetComponent<Health>();
                    _photonView2 = player.GetPhotonView();
                    _player = _photonView2.GetComponent<Fighter>();
                    _playerHealth = _photonView2.GetComponent<Health>();

                    AttackSkill();
                    break;
                }
            }
        }
        
        private void AttackSkill()
        {
            foreach (SkillStaticData data in _botData.SkillDatas)
            {
                if (SkillTypeId.Attack == data.Type)
                    _skillData = data;
            }
        }
    }
}
using System.Collections;
using Infrastructure.EnemyBot;
using Infrastructure.Hero;
using Photon.Pun;
using StaticData;
using UnityEngine;

namespace Logic
{
    public class EventAnimation : MonoBehaviour
    {
        [SerializeField] private Fighter _fighter;

        private PhotonView _photonView;
        private PhotonView _photonView2;
        private PlayerStaticData _playerData;
        private SkillStaticData _skillData;
        private Health _enemyHealth;
        private Health _playerHealth;
        private IFighter _enemy;
        private IFighter _bot;

        private void Start()
        {
            if (!_fighter.PhotonView.IsMine) return;

            _playerData = _fighter.PlayerData;
            _photonView = GetComponent<PhotonView>();

            StartCoroutine(CreateHero());
        }

        public void TakeDamage()
        {
            if (_fighter.CurrentSkill == SkillTypeId.Attack.ToString())
            {
                if (_bot == null)
                {
                    CompareSkills(_enemy, _enemyHealth, _playerHealth);
                }
                else
                {
                    CompareSkills(_bot, _enemyHealth, _playerHealth);
                }
            }
        }

        private void CompareSkills(IFighter fighter, Health enemyHealth, Health playerHealth)
        {
            if (fighter.CurrentSkill == SkillTypeId.Defence.ToString())
            {
                enemyHealth.ApplyDamage(_skillData.Damage / 2);
            }
            else if (fighter.CurrentSkill == SkillTypeId.Counterstrike.ToString())
            {
                enemyHealth.ApplyDamage(_skillData.Damage / 2);
                playerHealth.ApplyDamage(_skillData.Damage / 2);
            }
            else if (fighter.CurrentSkill == SkillTypeId.Evasion.ToString())
            {
            }
            else
                enemyHealth.ApplyDamage(_skillData.Damage);
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
                if (player.GetComponent<BotFighter>() != null)
                {
                    Debug.Log("target bot ");
                    TargetEnemy(player);
                    _bot = _photonView2.GetComponent<IFighter>();
                    break;
                }

                if (player != null)
                {
                    //GetFighters();
                    Debug.Log("target player ");
                    TargetEnemy(player);
                    _enemy = _photonView2.GetComponent<Fighter>();
                }
            }
        }

        private void TargetEnemy(GameObject player)
        {
            _playerHealth = _photonView.GetComponent<Health>();
            _photonView2 = player.GetPhotonView();
            _enemyHealth = _photonView2.GetComponent<Health>();
            AttackSkill();
        }

        // поиск игрока через PhotonNetwork.PlayerList (НЕ УДАЛЯТЬ)
        // private void GetFighters()
        // {
        //     Player[] hro3 = PhotonNetwork.PlayerList;
        //
        //     foreach (Player player in hro3)
        //     {
        //         if (player.ActorNumber != _photonView.OwnerActorNr)
        //         {
        //             int actorNr = player.ActorNumber;
        //
        //             for (int viewId = actorNr * PhotonNetwork.MAX_VIEW_IDS + 1;
        //                  viewId < (actorNr + 1) * PhotonNetwork.MAX_VIEW_IDS;
        //                  viewId++)
        //             {
        //                 _photonView2 = PhotonView.Find(viewId);
        //                 break;
        //             }
        //         }
        //     }
        // }

        private void AttackSkill()
        {
            foreach (SkillStaticData data in _playerData.SkillDatas)
            {
                if (SkillTypeId.Attack == data.Type)
                    _skillData = data;
            }
        }
    }
}
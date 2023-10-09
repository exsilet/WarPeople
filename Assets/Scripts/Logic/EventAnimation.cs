using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure.Hero;
using Photon.Pun;
using Photon.Realtime;
using StaticData;
using Unity.VisualScripting;
using UnityEngine;

namespace Logic
{
    [RequireComponent(typeof(Animator))]
    public class EventAnimation : MonoBehaviour
    {
        [SerializeField] private HeroHealth _heroHealth;
        [SerializeField] private Fighter _fighter;
        [SerializeField] private float _attackDistance;

        private PhotonView _photonView;
        private PhotonView _photonView2;
        private PlayerStaticData _playerData;
        private SkillStaticData _skillData;
        private HeroHealth _enemyHealth;
        private HeroHealth _playerHealth;
        private Fighter _enemy;
        private Fighter _player;

        private void Start()
        {
            if (!_fighter.PhotonView.IsMine) return;

            _playerData = _fighter.PlayerData;
            _photonView = GetComponent<PhotonView>();

            StartCoroutine(CreateHero());
        }

        public void TakeDamage()
        {           
            if (_player.CurrentSkill == SkillTypeId.Attack.ToString())
            {
                if (_enemy.CurrentSkill == SkillTypeId.Defence.ToString())
                {
                    _enemyHealth.ApplyDamage(_skillData.Damage / 2);
                }
                else if (_enemy.CurrentSkill == SkillTypeId.Counterstrike.ToString())
                {
                    _enemyHealth.ApplyDamage(_skillData.Damage / 2);
                    _playerHealth.ApplyDamage(_skillData.Damage / 2);
                }
                else if (_enemy.CurrentSkill == SkillTypeId.Evasion.ToString())
                {                    
                }
                else
                    _enemyHealth.ApplyDamage(_skillData.Damage);
            }
        }               

        // public void DamageHero()
        // {
        //     RaycastHit2D[] size = Physics2D.RaycastAll(transform.position, transform.right, _attackDistance);
        //
        //     Debug.DrawRay(transform.position, transform.right * _attackDistance, Color.red);
        //
        //     foreach (RaycastHit2D hit2D in size)
        //     {
        //         if (hit2D.collider != null)
        //         {
        //             Debug.Log("object 2" + hit2D.collider);
        //
        //             PhotonView photonView = hit2D.collider.gameObject.GetComponent<PhotonView>();
        //
        //             Debug.Log("PhotonView " + photonView.ViewID);
        //
        //             if (!photonView.IsMine)
        //             {
        //                 if (hit2D.collider.gameObject.TryGetComponent<HeroHealth>(out HeroHealth heroHealth))
        //                 {
        //                     Debug.Log("Damage hero " + hit2D.collider);
        //                     heroHealth.ApplyDamage(_damage);
        //                 }
        //             }
        //         }
        //     }
        // }

        private IEnumerator CreateHero()
        {
            yield return new WaitForSeconds(1f);
            GetFighters();
            _player = _photonView.GetComponent<Fighter>();
            _playerHealth = _photonView.GetComponent<HeroHealth>();
            _enemy = _photonView2.GetComponent<Fighter>();
            _enemyHealth = _photonView2.GetComponent<HeroHealth>();            

            AttackSkill();
        }

        private void GetFighters()
        {
            Player[] hro3 = PhotonNetwork.PlayerList;

            foreach (Player player in hro3)
            {
                if (player.ActorNumber != _photonView.OwnerActorNr)
                {
                    int actorNr = player.ActorNumber;

                    for (int viewId = actorNr * PhotonNetwork.MAX_VIEW_IDS + 1;
                         viewId < (actorNr + 1) * PhotonNetwork.MAX_VIEW_IDS;
                         viewId++)
                    {
                        _photonView2 = PhotonView.Find(viewId);
                        break;
                    }
                }
            }
        }

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
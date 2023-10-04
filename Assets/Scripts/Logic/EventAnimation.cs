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
        private PlayerStaticData _staticData;
        private SkillStaticData _attackSkill;

        private void Start()
        {
            if (!_fighter.PhotonView.IsMine) return;

            _staticData = _fighter.PlayerData;
            _photonView = GetComponent<PhotonView>();
            AttackSkill();
            
            StartCoroutine(CreateHero());
        }

        public void TakeDamage()
        {
            if (!_photonView2.IsMine)
            {
                if (_photonView2.gameObject.TryGetComponent<HeroHealth>(out HeroHealth heroHealth))
                {
                    heroHealth.ApplyDamage(_attackSkill.Damage);
                }
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
            foreach (SkillStaticData data in _staticData.SkillDatas)
            {
                if (_attackSkill.Damage == data.Damage)
                    _attackSkill = data;
            }
        }
    }
}
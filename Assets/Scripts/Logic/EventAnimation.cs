using System;
using Infrastructure.Hero;
using Photon.Pun;
using StaticData;
using UnityEngine;

namespace Logic
{
    [RequireComponent(typeof(Animator))]
    public class EventAnimation : MonoBehaviour
    {
        [SerializeField] private HeroHealth _heroHealth;
        [SerializeField] private Fighter _fighter;

        private PlayerStaticData _staticData;
        private SkillStaticData _attackSkill;

        private void Start()
        {
            if (!_fighter.PhotonView.IsMine) return;
            
            _staticData = _fighter.PlayerData;
            //ApplySkill();
        }

        public void TakeDamage()
        {
            _heroHealth.ApplyDamage(1, PhotonNetwork.LocalPlayer);
        }
    }
}
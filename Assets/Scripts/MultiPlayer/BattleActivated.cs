using System.Collections.Generic;
using Infrastructure.Factory;
using Infrastructure.Hero;
using Photon.Pun;
using UnityEngine;

namespace MultiPlayer
{
    public class BattleActivated : MonoBehaviour
    {
        private List<Fighter> _heroes = new();
        private IGameFactory _gameFactory;
        private Fighter _player1;
        private Fighter _player2;

        public void OnEnable()
        {
        }

        [PunRPC]
        public void AddHero(Fighter fighter)
        {
            
        }
    }
}
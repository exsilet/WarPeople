using System.Collections.Generic;
using Infrastructure.Factory;
using Infrastructure.Hero;
using Photon.Pun;
using UnityEngine;

namespace MultiPlayer
{
    public class BattleActivated : MonoBehaviour
    {
        private List<Player> _heroes = new();
        private IGameFactory _gameFactory;
        private Player _player1;
        private Player _player2;

        public void OnEnable()
        {
        }

        [PunRPC]
        public void AddHero(Player player)
        {
            
        }
    }
}
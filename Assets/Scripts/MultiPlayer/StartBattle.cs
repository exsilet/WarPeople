using System;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

namespace MultiPlayer
{
    public class StartBattle : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject _player;
        [SerializeField] private GameObject _secondPlayer;
        [SerializeField] private Transform _transformPlayer;
        [SerializeField] private Transform _transformSecondPlayer;

        private void Start()
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    CreatePlayer(_player, _transformPlayer);
                }
                else
                {
                    CreatePlayer(_secondPlayer, _transformSecondPlayer);
                }
            }
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.LogFormat("Player {0} enter room", newPlayer.NickName);
        }

        private void CreatePlayer(GameObject player, Transform transformPlayer)
        {
            PhotonNetwork.Instantiate(player.name, transformPlayer.position, Quaternion.identity);
        }
    }
}
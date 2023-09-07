using System;
using Photon.Pun;
using UnityEngine;

namespace MultiPlayer
{
    public class StartBattle : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject _player;
        [SerializeField] private Transform _transformPlayer;

        private void Start()
        {
             GameObject player = PhotonNetwork.Instantiate(_player.name, _transformPlayer.position, Quaternion.identity);
             player.transform.SetParent(_transformPlayer);
        }
        
        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.LogFormat("Player {0} enter room", newPlayer.NickName);
        }
    }
}
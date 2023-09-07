using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MultiPlayer
{
    public class StartServer : MonoBehaviourPunCallbacks
    {
        [SerializeField] private int _maxPlayer;

        private const string GameScene = "GameScene";
        
        private void Start()
        {
            PhotonNetwork.NickName = "Player " + Random.Range(1000, 9999);
            Debug.Log("Player's name is set to " + PhotonNetwork.NickName);
            
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "1";
            PhotonNetwork.ConnectUsingSettings();
        }

        public void QuickMatch()
        {
            CreateRoom();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to master");
        }

        private void CreateRoom()
        {
            RoomOptions roomOptions = new RoomOptions()
            {
                MaxPlayers = _maxPlayer
            };

            PhotonNetwork.CreateRoom(null, roomOptions);
        }

        public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

        public override void OnJoinedRoom()
        {
            Debug.Log("Connected to room");
            
            PhotonNetwork.LoadLevel(GameScene);
        }
    }
}
using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace MultiPlayer
{
    public class StartServer : MonoBehaviourPunCallbacks
    {
        [SerializeField] private byte _maxPlayer;
        [SerializeField] private TMP_Text _connectionStatus;
        [SerializeField] private TMP_Text _playerNameText;
        [SerializeField] private float _timerStart;
        [SerializeField] private TMP_Text _textTimer;
        [SerializeField] private GameObject _panel;

        private string _playerName;
        private const string GameScene = "GameScene";

        private void Start()
        {
            ConnectToPhotonServer();
        }

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void ConnectToPhotonServer()
        {
            _connectionStatus.text = "Connecting...";
            PhotonNetwork.GameVersion = "1";
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnected()
        {
            base.OnConnected();

            _connectionStatus.text = "Connected to Photon!";
            _connectionStatus.color = Color.green;
        }

        public void SetPlayerName()
        {
            _playerName = _playerNameText.text;
        }

        public void QuickMatch()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to master");
        }

        private void CreateRoom()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = _playerName;
                RoomOptions roomOptions = new RoomOptions()
                {
                    MaxPlayers = _maxPlayer
                };

                if (roomOptions.MaxPlayers >= _maxPlayer)
                {
                    PhotonNetwork.CreateRoom(null, roomOptions);
                }
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            CreateRoom();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Connected to room");
            StartCoroutine(ActivePlayer());
        }

        private IEnumerator ActivePlayer()
        {
            while (PhotonNetwork.CurrentRoom.PlayerCount != 2)
            {
                SearchTime();
                yield return null;
            }

            OnEnd();
        }

        private void SearchTime()
        {
            _panel.gameObject.SetActive(true);
            _timerStart += Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(_timerStart);
            _textTimer.text = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
        }

        private void OnEnd()
        {
            StopCoroutine(ActivePlayer());
            PhotonNetwork.LoadLevel(GameScene);
        }
    }
}
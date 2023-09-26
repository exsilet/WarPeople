using System;
using System.Collections;
using Assets.Scripts.Infrastructure.UI.Menu;
using Infrastructure.LevelLogic;
using Infrastructure.Services;
using Infrastructure.States;
using Photon.Pun;
using Photon.Realtime;
using StaticData;
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
        [SerializeField] private ChooseFighter _chooseFighter;
        
        private PlayerStaticData _playerStaticData;
        private string _playerName;
        private IGameStateMachine _stateMachine;
        private const string GameScene = "GameScene";
        private PlayerStaticData _staticData;

        private void Start()
        {
            ConnectToPhotonServer();
            _playerStaticData = _chooseFighter.CurrentFighter;
        }

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
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
        public void SetPlayerData(PlayerStaticData staticData)
        {
            _playerStaticData = staticData;
        }

        public void QuickMatch()
        {
            _staticData = _playerStaticData;
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
                    CleanupCacheOnLeave = false,
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
            // PhotonNetwork.LoadLevel(GameScene);
            //_stateMachine.Enter<LoadLevelState, string>(GameScene, _staticData);
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
            _panel.SetActive(true);
            _timerStart += Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(_timerStart);
            _textTimer.text = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
        }

        private void OnEnd()
        {
            StopCoroutine(ActivePlayer());
            //PhotonNetwork.LoadLevel(GameScene);
            _stateMachine.Enter<LoadLevelState, string>(GameScene, _staticData);
        }
    }
}
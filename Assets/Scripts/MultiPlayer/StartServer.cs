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
        
        private PlayerStaticData _choosedPlayerData;
        private PlayerStaticData _staticData;
        private PlayerStaticData _botData;
        private IGameStateMachine _stateMachine;
        private string _playerName;
        private const string GameScene = "GameScene";

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
        }

        private void Start()
        {
            ConnectToPhotonServer();
            _choosedPlayerData = _chooseFighter.CurrentFighter;
        }

        //private void Update()
        //{
        //    if (_timerStart == 2)
        //    {
        //        OnEnd();
        //    }
        //}

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
            _choosedPlayerData = staticData;
        }

        public void QuickMatch()
        {
            _staticData = _choosedPlayerData;
            _botData = _chooseFighter.GetRandomData();
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
            //_stateMachine.Enter<LoadLevelState, string>(GameScene, _staticData, null);
            StartCoroutine(ActivePlayer());
        }

        private IEnumerator ActivePlayer()
        {
            while (PhotonNetwork.CurrentRoom.PlayerCount != 2)
            {
                SearchTime();
                if (_timerStart >= 4)
                {
                    EnterOnePlayer();
                    //break;
                }

                yield return null;
            }
            EnterTowPlayers();
        }

        private void EnterTowPlayers()
        {
            StopCoroutine(ActivePlayer());
            _stateMachine.Enter<LoadLevelState, string>(GameScene, _staticData, null);
        }

        private void SearchTime()
        {
            _panel.SetActive(true);
            _timerStart += Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(_timerStart);
            _textTimer.text = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");            
        }

        private void EnterOnePlayer()
        {
            StopCoroutine(ActivePlayer());
            Debug.Log(_botData);
            _stateMachine.Enter<LoadLevelState, string>(GameScene, _staticData, _botData);
        }
    }
}
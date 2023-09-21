using System;
using Infrastructure.Factory;
using Infrastructure.States;
using Logic;
using Photon.Pun;
using StaticData;
using UnityEngine;

namespace Infrastructure.LevelLogic
{
    public class LoadLevelState : IPayloadedState1<string, PlayerStaticData>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IStaticDataService _staticData;
        private PlayerStaticData _playerData;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
            IGameFactory gameFactory, IStaticDataService staticData)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _staticData = staticData;
        }

        public void EnterTwoParameters(string sceneName, PlayerStaticData playerData)
        {
            _loadingCurtain.Show();
            _playerData = playerData;
            Debug.Log(_playerData + " save data player");
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        private void OnLoaded()
        {
            InitGameWorld();
            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit() =>
            _loadingCurtain.Hide();

        private void InitGameWorld()
        {
            CreateHeroWorld(_playerData);
        }

        private void CreateHeroWorld(PlayerStaticData playerData)
        {
            Debug.Log(playerData + " create data players");
            
            GameObject hero = _gameFactory.CreateHero(playerData.PlayerTypeId, playerData);
        }
    }
}
using Infrastructure.Factory;
using Infrastructure.States;
using Logic;
using StaticData;
using UnityEngine;

namespace Infrastructure.LevelLogic
{
    public class LoadLevelState : IPayloadedState1<string, PlayerStaticData, PlayerStaticData>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IStaticDataService _staticData;
        private PlayerStaticData _playerData;
        private PlayerStaticData _botData;

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

        public void EnterThreeParameters(string sceneName, PlayerStaticData playerData, PlayerStaticData botData)
        {
            _loadingCurtain.Show();
            _playerData = playerData;
            _botData = botData;
            _sceneLoader.Load(sceneName, OnLoaded);
            Debug.Log(_playerData + " save data player");
            Debug.Log(_botData + " save data player2");
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
            Debug.Log(_botData);
            if (_botData != null)
            {
                CreateOffline(_playerData, _botData);
            }
            else
                CreateHeroWorld(_playerData);
        }

        private void CreateHeroWorld(PlayerStaticData playerData)
        {
            Debug.Log(playerData + " create data players");
            
            GameObject hero = _gameFactory.CreateHero(playerData);
        }

        private void CreateOffline(PlayerStaticData playerData, PlayerStaticData botData)
        {
            GameObject hero2 = _gameFactory.CreateBot(botData);
            GameObject hero1 = _gameFactory.CreateHeroOffline(playerData);
        }        
    }
}
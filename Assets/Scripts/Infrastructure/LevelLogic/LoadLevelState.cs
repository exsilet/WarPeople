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
        private PlayerStaticData _playerStatic;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
            IGameFactory gameFactory, IStaticDataService staticData)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _staticData = staticData;
        }

        public void EnterTwoParameters(string sceneName, PlayerStaticData namePlayer)
        {
            _loadingCurtain.Show();
            _playerStatic = namePlayer;
            Debug.Log(_playerStatic + " save data player");
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
            CreateHeroWorld(_playerStatic);
        }

        private void CreateHeroWorld(PlayerStaticData typeID)
        {
            Debug.Log(typeID + " create data players");
            GameObject hero = _gameFactory.CreateHero(typeID.PlayerTypeId);
            GameObject hud = _gameFactory.CreateHudBattle();

            hero.GetComponent<Player>().Construct(hud.GetComponentInChildren<SkillsPanel>());
            hero.GetComponentInChildren<Inventory>().Construct(hud.GetComponentInChildren<SkillsPanel>());

            InitHud(hud, hero);
        }

        private static void InitHud(GameObject hud, GameObject hero)
        {
            hud.GetComponentInChildren<SkillsPanel>().Construct(hero.GetComponentInChildren<Inventory>());
        }
    }
}
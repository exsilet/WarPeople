using Logic;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace Infrastructure
{
    public class LoadMenuState : IPayloadedState<string>
    {
        private const string HudPath = "Hud/Hud";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;

        public LoadMenuState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() => 
            _curtain.Hide();

        private void OnLoaded()
        {
            GameObject hud = Resources.Load<GameObject>(HudPath);
            _stateMachine.Enter<GameLoopState>();
        }
    }
}
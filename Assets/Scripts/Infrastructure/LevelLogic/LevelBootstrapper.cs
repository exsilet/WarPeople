using Infrastructure.Services;
using Infrastructure.States;
using Infrastructure.UI;
using Logic;
using UnityEngine;

namespace Infrastructure.LevelLogic
{
    public class LevelBootstrapper : MonoBehaviour
    {
        [SerializeField] private LevelScreen _levelScreen;
        
        private const string Battle = "GameScene";
        private IGameStateMachine _stateMachine;
        
        private void Awake()
        {
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
        }
        
        private void OnEnable()
        {
            _levelScreen.BattleLoaded += OnAcademyLoaded;
        }


        private void OnDisable()
        {
            _levelScreen.BattleLoaded -= OnAcademyLoaded;
        }        

        private void OnAcademyLoaded()
        {
            _stateMachine.Enter<LoadMenuState, string>(Battle);
        }
    }
}
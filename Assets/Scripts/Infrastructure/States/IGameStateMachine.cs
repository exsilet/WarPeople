using Infrastructure.Services;
using StaticData;

namespace Infrastructure.States
{
    public interface IGameStateMachine : IService
    {
        void Enter<TState>() where TState : class, IState;
        void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;
        void Enter<TState, TPayload>(TPayload payload1, PlayerStaticData payload2) where TState : class, IPayloadedState1<TPayload, PlayerStaticData>;
    }
}
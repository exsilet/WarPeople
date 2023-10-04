namespace Infrastructure.States
{
    public interface IState : IExitableState
    {
        void Enter();
    }

    public interface IPayloadedState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }
    
    public interface IPayloadedState1<TPayload, TPayload1, TPayload2> : IExitableState
    {
        void EnterThreeParameters(TPayload payload, TPayload1 payload1, TPayload2 payload2);        
    }    

    public interface IExitableState
    {
        void Exit();
    }
}
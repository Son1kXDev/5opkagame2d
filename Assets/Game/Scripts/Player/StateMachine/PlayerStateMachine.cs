public class PlayerStateMachine
{
    public PlayerState CurrentPlayerState { get; private set; }

    public void Initialize(PlayerState initialState)
    {
        CurrentPlayerState = initialState;
        CurrentPlayerState.EnterState();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentPlayerState.ExitState();
        CurrentPlayerState = newState;
        CurrentPlayerState.EnterState();
    }

}

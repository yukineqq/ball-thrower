using UnityEngine;

public sealed class GolfRestartSessionState : GolfState
{
    public GolfRestartSessionState(GolfStateMachine stateMachine, GolfReusableData reusableData, GolfBoard board) : base(stateMachine, reusableData, board)
    {

    }

    public override void Enter()
    {
        base.Enter();

        RestartSession();

        _stateMachine.EnterState<GolfBeforeTouchState>();
    }

    private void RestartSession()
    {

    }
}

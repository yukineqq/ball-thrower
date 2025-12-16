using UnityEngine;

public sealed class GolfPreparationState : GolfState
{
    public GolfPreparationState(GolfStateMachine stateMachine, GolfReusableData reusableData) : base(stateMachine, reusableData)
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
        _reusableData.Timer.SetSecondsLeftAmount(30f);

        _reusableData.Timer.SetEnabled(true);
    }
}

using UnityEngine;

public abstract class GolfState : SyncBaseState
{
    protected readonly GolfStateMachine _stateMachine;
    protected readonly GolfReusableData _reusableData;
    protected readonly GolfBoard _board;

    public GolfState(GolfStateMachine stateMachine, GolfReusableData reusableData, GolfBoard board)
    {
        _stateMachine = stateMachine;
        _reusableData = reusableData;
        _board = board;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log($"entered {this.GetType().Name}");
    }

    protected void AllowGolfHoleModeTransitions()
    {
        foreach (var hole in _reusableData.GolfHoles)
        {
            hole.AllowModeTransition();
        }
    }

    protected void PreventGolfHoleModeTransitions()
    {
        foreach (var hole in _reusableData.GolfHoles)
        {
            hole.PreventModeTransition();
        }
    }
}

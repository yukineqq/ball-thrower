using UnityEngine;

public abstract class GolfState : SyncBaseState
{
    protected readonly GolfStateMachine _stateMachine;
    protected readonly GolfReusableData _reusableData;

    public GolfState(GolfStateMachine stateMachine, GolfReusableData reusableData)
    {
        _stateMachine = stateMachine;
        _reusableData = reusableData;
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

    protected override void AddSubscriptions()
    {
        base.AddSubscriptions();

        _reusableData.Timer.SecondsLeftChanged += OnTimerValueChanged;
    }

    protected override void RemoveSubscriptions()
    {
        base.RemoveSubscriptions();

        _reusableData.Timer.SecondsLeftChanged -= OnTimerValueChanged;
    }

    private void OnTimerValueChanged(float value)
    {
        if (value <= 0f)
        {
            _reusableData.Timer.SetEnabled(false);

            RemoveSubscriptions();

            _stateMachine.EnterState<GolfFinishState>();
        }
    }
}

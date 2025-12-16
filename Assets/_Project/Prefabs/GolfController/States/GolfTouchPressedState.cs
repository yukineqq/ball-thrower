using UnityEngine;

public sealed class GolfTouchPressedState : GolfState
{
    private readonly IInputHandler _input;

    public GolfTouchPressedState(GolfStateMachine stateMachine, GolfReusableData reusableData, GolfBoard board, IInputHandler inputHandler) : base(stateMachine, reusableData, board)
    {
        _input = inputHandler;
    }

    public override void Enter()
    {
        base.Enter();

        PreventGolfHoleModeTransitions();
    }

    protected override void AddSubscriptions()
    {
        base.AddSubscriptions();

        _input.PrimaryTapPerformed += OnPressPerformed;
        _input.TouchPositionChanged += OnPressPositionChanged;
    }

    protected override void RemoveSubscriptions()
    {
        base.RemoveSubscriptions();

        _input.PrimaryTapPerformed -= OnPressPerformed;
        _input.TouchPositionChanged -= OnPressPositionChanged;
    }

    private void OnPressPerformed(Vector2 position)
    {
        _reusableData.CurrentTouchReleasePosition = position;

        _stateMachine.EnterState<GolfTouchReleaseState>();
    }

    private void OnPressPositionChanged(Vector2 position)
    {
        Vector2 differenceVector = _reusableData.CurrentTouchStartPosition - position;

        Debug.DrawLine(Vector3.zero, differenceVector.normalized, Color.magenta);
    }
}

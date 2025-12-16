using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class GolfTouchReleaseState : GolfState
{
    public GolfTouchReleaseState(GolfStateMachine stateMachine, GolfReusableData reusableData, GolfBoard board) : base(stateMachine, reusableData, board)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        AllowGolfHoleModeTransitions();

        Vector2 direction = (_reusableData.CurrentTouchStartPosition - _reusableData.CurrentTouchReleasePosition);
        direction = Vector2.ClampMagnitude(direction, 1f);

        _reusableData.Ball.Shoot(new Vector3(direction.x, 0f, direction.y), 15f);

        _reusableData.Ball.HoleHit += OnHit;
        _reusableData.Ball.Timeout += OnTimeout;
    }

    public override void Exit()
    {
        base.Exit();

        _reusableData.CurrentTouchStartPosition = Vector2.zero;
        _reusableData.CurrentTouchReleasePosition = Vector2.zero;
    }

    protected override void RemoveSubscriptions()
    {
        base.RemoveSubscriptions();

        _reusableData.Ball.HoleHit -= OnHit;
        _reusableData.Ball.Timeout -= OnTimeout;
    }

    private void OnHit(GolfHole hole)
    {
        RemoveSubscriptions();

        _stateMachine.EnterState<GolfBeforeTouchState>();
    }

    private void OnTimeout()
    {
        RemoveSubscriptions();

        _stateMachine.EnterState<GolfBeforeTouchState>();
    }
}

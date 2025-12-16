using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class GolfTouchReleaseState : GolfState
{
    private readonly ISavingService _savingService;

    public GolfTouchReleaseState(GolfStateMachine stateMachine, GolfReusableData reusableData, ISavingService savingService) : base(stateMachine, reusableData)
    {
        _savingService = savingService;
    }

    public override void Enter()
    {
        base.Enter();

        AllowGolfHoleModeTransitions();

        Vector2 direction = (_reusableData.CurrentTouchStartPosition - _reusableData.CurrentTouchReleasePosition);
        direction = Vector2.ClampMagnitude(direction, 1f);

        _reusableData.Ball.Shoot(new Vector3(direction.x, 0f, direction.y), _reusableData.ShootingForce, _reusableData.BallTimeoutDelay);

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

        switch (hole.IsNegative)
        {
            case true:
                _reusableData.Timer.SubtractSeconds(10f);
                _reusableData.SessionHelper.DecrementScore();
                break;
            default:
                _reusableData.Timer.AddSeconds(5f);
                _reusableData.SessionHelper.IncrementScore();
                _reusableData.SessionHelper.AddBalance(1);
                break;
        }

        _stateMachine.EnterState<GolfBeforeTouchState>();
    }

    private void OnTimeout()
    {
        RemoveSubscriptions();

        _stateMachine.EnterState<GolfBeforeTouchState>();
    }
}

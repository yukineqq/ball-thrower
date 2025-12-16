using UnityEngine;

public sealed class GolfTouchPressedState : GolfState
{
    private readonly ISpawnService _spawnService;
    private readonly IInputHandler _input;
    private TrajectoryLine _trajectoryLine;

    public GolfTouchPressedState(GolfStateMachine stateMachine, GolfReusableData reusableData, ISpawnService spawnService, IInputHandler inputHandler) : base(stateMachine, reusableData)
    {
        _spawnService = spawnService;
        _input = inputHandler;
    }

    public override void Enter()
    {
        base.Enter();

        _trajectoryLine = _spawnService.SpawnFromPool<TrajectoryLine>();
        //PreventGolfHoleModeTransitions();
    }

    public override void Exit()
    {
        base.Exit();

        _spawnService.ReleaseObject(_trajectoryLine);
        _trajectoryLine = null;
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
        _trajectoryLine.ShootLaser(_reusableData.CurrentTouchStartPosition, position, _reusableData.Ball.transform);
    }
}

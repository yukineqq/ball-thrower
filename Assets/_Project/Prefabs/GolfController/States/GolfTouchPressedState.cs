using Unity.VisualScripting;
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
        Vector2 differenceVector = _reusableData.CurrentTouchStartPosition - position;
        Vector3 direction = new Vector3(differenceVector.x, 0f, differenceVector.y).normalized;

        float power = 10f;
        Vector3 initialVelocity = direction * power;

        Transform ballTransform = _reusableData.Ball.transform;

        _trajectoryLine.transform.position = new Vector3(ballTransform.position.x, 0.1f, ballTransform.position.z);

        int trajectoryLength = 15;
        LineRenderer lineRenderer = _trajectoryLine.LineRenderer;
        lineRenderer.positionCount = trajectoryLength;

        Vector3 velocity = initialVelocity;
        Vector3 positionAtTime = ballTransform.position;

        for (int i = 0; i < trajectoryLength; i++)
        {
            Vector3 newPosition = positionAtTime + velocity * 0.1f;

            RaycastHit hit;
            if (Physics.Raycast(positionAtTime, velocity, out hit, Vector3.Distance(positionAtTime, newPosition)))
            {
                lineRenderer.SetPosition(i, hit.point);
                break;
            }

            lineRenderer.SetPosition(i, newPosition);
            positionAtTime = newPosition;
        }
    }
}

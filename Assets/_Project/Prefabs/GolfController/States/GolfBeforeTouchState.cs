using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public sealed class GolfBeforeTouchState : GolfState
{
    private readonly IInputHandler _input;

    public GolfBeforeTouchState(GolfStateMachine stateMachine, GolfReusableData reusableData, GolfBoard board, IInputHandler inputHandler) : base(stateMachine, reusableData, board)
    {
        _input = inputHandler;
    }

    public override void Enter()
    {
        base.Enter();

        _reusableData.Ball.gameObject.SetActive(true);
        _reusableData.Ball.Teleport(_board.BallSpawnpoint);
        _reusableData.Ball.SetIsKinematic(true);

        ShuffleGolfHoles();

        AllowGolfHoleModeTransitions();
    }

    protected override void AddSubscriptions()
    {
        base.AddSubscriptions();

        _input.PrimaryTapStarted += OnPressStarted;
    }

    protected override void RemoveSubscriptions()
    {
        base.RemoveSubscriptions();

        _input.PrimaryTapStarted -= OnPressStarted;
    }

    private void OnPressStarted(Vector2 position)
    {
        _reusableData.CurrentTouchStartPosition = new Vector2(Screen.width / 2, Screen.height / 4);//position; Debug.Log($"start position: {_reusableData.CurrentTouchStartPosition}");

        _stateMachine.EnterState<GolfTouchPressedState>();
    }

    private void ShuffleGolfHoles()
    {
        List<Vector3> spawnPositions = _board.GetSpawnPositions();

        foreach (var golfHole in _reusableData.GolfHoles)
        {

            if (spawnPositions.Count <= 0)
            {
                throw new System.Exception("there is no available positions to spawn golf holes");
            }

            Vector3 positionToSpawnAt = spawnPositions[Random.Range(0, spawnPositions.Count)];

            spawnPositions.Remove(positionToSpawnAt);

            golfHole.transform.position = positionToSpawnAt;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public sealed class GolfStateMachine : SyncStateMachine<GolfState>, ITickable
{
    private readonly IInputHandler _inputHandler;
    private readonly ISpawnService _spawnService;
    private readonly GolfTimer _timer;
    private readonly GolfReusableData _reusableData = new GolfReusableData();

    public GolfStateMachine(IInputHandler inputHandler, ISpawnService spawnService, GolfTimer timer)
    {
        _inputHandler = inputHandler;
        _spawnService = spawnService;
        _timer = timer;
    }

    public void Setup(GolfBoard board, int golfHolesAmount)
    {
        if (board == null)
        {
            throw new System.Exception("board is null");
        }

        ShootingBall ball = _spawnService.SpawnFromPool<ShootingBall>();
        ball.gameObject.SetActive(false);

        List<GolfHole> golfHoles = new List<GolfHole>();

        for (int i = 0; i < golfHolesAmount; i++)
        {
            golfHoles.Add(_spawnService.SpawnFromPool<GolfHole>());
        }

        _reusableData.Board = board;
        _reusableData.GolfHoles = golfHoles;
        _reusableData.Ball = ball;
        _reusableData.Timer = _timer;

        RegisterState<GolfBeforeTouchState>(new GolfBeforeTouchState(this, _reusableData, board, _inputHandler));
        RegisterState<GolfTouchPressedState>(new GolfTouchPressedState(this, _reusableData, board, _inputHandler));
        RegisterState<GolfTouchReleaseState>(new GolfTouchReleaseState(this, _reusableData, board));
        RegisterState<GolfRestartSessionState>(new GolfRestartSessionState(this, _reusableData, board));
    }

    public void Tick()
    {
        UpdateState();
    }
}

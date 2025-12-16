using System.Collections.Generic;
using UnityEngine;
using Zenject;

public sealed class GolfStateMachine : SyncStateMachine<GolfState>, IInitializable, ITickable
{
    private readonly IInputHandler _inputHandler;
    private readonly ISpawnService _spawnService;
    private readonly ISavingService _savingService;
    private readonly SceneStateMachine _sceneStateMachine;
    private readonly PlayingSessionHelper _sessionHelper;
    private readonly GolfTimer _timer;
    private GolfReusableData _reusableData;

    public GolfStateMachine(IInputHandler inputHandler, ISpawnService spawnService, ISavingService savingService, 
        SceneStateMachine sceneStateMachine, PlayingSessionHelper sessionHelper, GolfTimer timer)
    {
        _inputHandler = inputHandler;
        _spawnService = spawnService;
        _savingService = savingService;
        _sceneStateMachine = sceneStateMachine;
        _sessionHelper = sessionHelper;
        _timer = timer;
    }

    public void Initialize()
    {
        ShootingBall ball = _spawnService.SpawnFromPool<ShootingBall>();
        GolfBoard board = _spawnService.Spawn<GolfBoard>(typeof(GolfBoard).Name);

        _reusableData = new GolfReusableData(board, ball, _timer, _sessionHelper);
        _reusableData.Ball.gameObject.SetActive(false);

        RegisterState<GolfBeforeTouchState>(new GolfBeforeTouchState(this, _reusableData, _inputHandler));
        RegisterState<GolfTouchPressedState>(new GolfTouchPressedState(this, _reusableData, _spawnService, _inputHandler));
        RegisterState<GolfTouchReleaseState>(new GolfTouchReleaseState(this, _reusableData, _savingService));
        RegisterState<GolfPreparationState>(new GolfPreparationState(this, _reusableData));
        RegisterState<GolfFinishState>(new GolfFinishState(this, _reusableData, _sceneStateMachine));
    }

    public void Setup(Vector2 boardSize, int golfHolesAmount, float shootingForce, float timeoutDelay)
    {
        _reusableData.Board.SetDimensions(boardSize.x, boardSize.y);
        _reusableData.GolfHolesAmount = golfHolesAmount;

        _reusableData.BallTimeoutDelay = timeoutDelay;
        _reusableData.ShootingForce = shootingForce;

        foreach (GolfHole golfHole in _reusableData.GolfHoles)
        {
            _spawnService.ReleaseObject(golfHole);
        }

        _reusableData.GolfHoles.Clear();

        for (int i = 0; i < _reusableData.GolfHolesAmount; i++)
        {
            _reusableData.GolfHoles.Add(_spawnService.SpawnFromPool<GolfHole>());
        }
    }

    public void Tick()
    {
        UpdateState();
    }
}

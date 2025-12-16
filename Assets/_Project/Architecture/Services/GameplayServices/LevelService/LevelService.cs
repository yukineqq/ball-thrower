using UnityEngine;
using Zenject;

public sealed class LevelService : IInitializable
{
    private readonly IStaticDataService _staticDataService;
    private readonly PlayerManager _playerManager;
    private readonly GolfStateMachine _golfStateMachine;
    private LevelServiceConfig _config;

    public LevelService(IStaticDataService staticDataService, PlayerManager playerManager, GolfStateMachine golfStateMachine)
    {
        _staticDataService = staticDataService;
        _playerManager = playerManager;
        _golfStateMachine = golfStateMachine;
    }

    public void Initialize()
    {
        _config = _staticDataService.GetResourcesSingleConfigByPath<LevelServiceConfig>("Configs/Infrastructure/LevelServiceConfig");
    }

    public void AutoSetupLevel()
    {
        _playerManager.SpawnPlayer(_config.CameraPosition, Quaternion.Euler(_config.CameraRotation));

        BuildBoard();
    }

    private void BuildBoard()
    {
        _golfStateMachine.Setup(_config.BoardSize, _config.GolfHolesCount, _config.ShootingForce, _config.BallTimeoutDelay);

        _golfStateMachine.EnterState<GolfPreparationState>();
    }
}

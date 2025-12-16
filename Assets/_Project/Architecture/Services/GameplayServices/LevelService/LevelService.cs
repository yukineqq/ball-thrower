using UnityEngine;
using Zenject;

public sealed class LevelService : IInitializable
{
    private readonly IStaticDataService _staticDataService;
    private readonly ISpawnService _spawnService;
    private readonly PlayerManager _playerManager;
    private readonly GolfStateMachine _golfStateMachine;
    private LevelServiceConfig _config;

    private GolfBoard _board;

    public LevelService(IStaticDataService staticDataService, ISpawnService spawnService, PlayerManager playerManager, GolfStateMachine golfStateMachine)
    {
        _staticDataService = staticDataService;
        _spawnService = spawnService;
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
        if (_board == null)
        {
            _board = _spawnService.Spawn<GolfBoard>(typeof(GolfBoard).Name);
        }

        _board.SetDimensions(_config.BoardSize.x, _config.BoardSize.y);

        _golfStateMachine.Setup(_board, _config.GolfHolesCount);

        _golfStateMachine.EnterState<GolfBeforeTouchState>();
    }
}

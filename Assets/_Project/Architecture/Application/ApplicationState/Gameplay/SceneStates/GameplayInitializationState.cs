using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public sealed class GameplayInitializationState : BaseSceneState
{
    private readonly SceneRootUI _sceneUi;
    private readonly GameplayUIManager _manager;
    private readonly PlayerManager _playerManager;
    private readonly IInputService _inputService;

    public GameplayInitializationState(GameStateMachine gameStateMachine, SceneStateMachine sceneStateMachine,
        UIRootView uiRoot, SceneRootUI sceneUi, GameplayUIManager manager,
        PlayerManager playerManager, IInputService inputService) : base(gameStateMachine, sceneStateMachine, uiRoot)
    {
        _sceneUi = sceneUi;
        _manager = manager;
        _playerManager = playerManager;
        _inputService = inputService;
    }

    public override async UniTask Enter()
    {
        await base.Enter();

        _uiRoot.ShowLoadingScreen();

        _inputService.ShowPointer(false);
        _inputService.Disable();

        InitializeUI();

        InitializePlayer();

        _uiRoot.HideLoadingScreen();

        _sceneStateMachine.Enter<GameplayPreparationState>().Forget();
    }

    private void InitializePlayer()
    {
        _playerManager.EnsurePlayerCreation();
    }

    private void InitializeUI()
    {
        _sceneUi.Cleanup();

        _manager.CreateScreen<GameplayScreenPresenter>();
        _manager.CreateScreen<GameplayStatusScreenPresenter>();
    }
}

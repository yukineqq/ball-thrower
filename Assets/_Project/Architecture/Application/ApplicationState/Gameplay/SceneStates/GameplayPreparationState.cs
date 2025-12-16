using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public sealed class GameplayPreparationState : BaseSceneState
{
    private readonly CancellationService _cancellationService;
    private readonly SceneRootUI _sceneUI;
    private readonly ISpawnService _spawnService;
    private readonly LevelService _levelService;
    private readonly IInputService _inputService;

    public GameplayPreparationState(CancellationService cancellationService, GameStateMachine gameStateMachine, SceneStateMachine sceneStateMachine, UIRootView uiRoot, SceneRootUI sceneUI,
        ISpawnService spawnService, LevelService levelService, IInputService inputService) : base(gameStateMachine, sceneStateMachine, uiRoot)
    {
        _cancellationService = cancellationService;
        _sceneUI = sceneUI;
        _spawnService = spawnService;
        _levelService = levelService;
        _inputService = inputService;
    }

    public override async UniTask Enter()
    {
        await base.Enter();

        _uiRoot.ShowLoadingScreen();

        _inputService.Disable();
        _inputService.ShowPointer(false);

        _sceneUI.CloseAllPopups();

        await PrepareLevel(_cancellationService.CurrentGlobalContextCancellationToken);

        _uiRoot.HideLoadingScreen();

        _sceneStateMachine.Enter<GameplayRegularState>().Forget();
    }

    private async UniTask PrepareLevel(CancellationToken cancellationToken)
    {
        //_spawnService.ClearSpawnedObjetcs();

        //_spawnService.ReleaseAllActiveItems();

        _levelService.AutoSetupLevel();

        await UniTask.Yield(cancellationToken);
    }
}

using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class GameLoadingState : BaseGameState
{
    private readonly CancellationService _cancellationService;
    private readonly ISceneLoader _sceneLoader;

    public GameLoadingState(GameStateMachine gameStateMachine, UIRootView uiRoot, CancellationService cancellationService, ISceneLoader sceneLoader) : base(gameStateMachine, uiRoot)
    {
        _cancellationService = cancellationService;
        _sceneLoader = sceneLoader;
    }

    public override async UniTask Enter()
    {
        await base.Enter();

        _uiRoot.ShowLoadingScreen();

        await _sceneLoader.ToLoadingScene(_cancellationService.CurrentGlobalContextCancellationToken);
    }

    public override async UniTask Exit()
    {
        await base.Exit();

        _uiRoot.ShowLoadingScreen();
    }
}

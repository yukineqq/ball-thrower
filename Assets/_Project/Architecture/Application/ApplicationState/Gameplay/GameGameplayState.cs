using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class GameGameplayState : BaseGameState
{
    private readonly CancellationService _cancellationService;
    private readonly ISceneLoader _sceneLoader;
    private readonly IAssetProvider _assetProvider;

    public GameGameplayState(GameStateMachine gameStateMachine, UIRootView uiRoot, CancellationService cancellationService,
        ISceneLoader sceneLoader, IAssetProvider assetProvider) : base(gameStateMachine, uiRoot)
    {
        _cancellationService = cancellationService;
        _sceneLoader = sceneLoader;
        _assetProvider = assetProvider;
    }

    public override async UniTask Enter()
    {
        await base.Enter();

        _uiRoot.ShowLoadingScreen();

        _cancellationService.UpdateCurrentContextCancellationToken();

        await WarmUpAssets();

        await _sceneLoader.Load(SceneNames.GAMEPLAYCORE);

        _uiRoot.HideLoadingScreen();
    }

    public override async UniTask Exit()
    {
        await base.Exit();

        Debug.Log("global gameplay state exit");

        await ReleaseAssets();
    }

    private async UniTask WarmUpAssets()
    {
        await _assetProvider.WarmupAssetsByLabel(AssetLabels.Config);
        await _assetProvider.WarmupAssetsByLabel(AssetLabels.GameplayState);
    }

    private async UniTask ReleaseAssets()
    {
        await _assetProvider.ReleaseAssetsByLabel(AssetLabels.Config);
        await _assetProvider.ReleaseAssetsByLabel(AssetLabels.GameplayState);
    }
}

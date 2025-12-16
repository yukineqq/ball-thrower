using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class GameMainMenuState : BaseGameState
{
    private readonly ISceneLoader _sceneLoader;
    private readonly IAssetProvider _assetProvider;

    public GameMainMenuState(GameStateMachine gameStateMachine, UIRootView uiRoot, ISceneLoader sceneLoader, IAssetProvider assetProvider) : base(gameStateMachine, uiRoot)
    {
        _sceneLoader = sceneLoader;
        _assetProvider = assetProvider;
    }

    public override async UniTask Enter()
    {
        await base.Enter();

        _uiRoot.ShowLoadingScreen();

        await _assetProvider.WarmupAssetsByLabel(AssetLabels.MainMenuState);
        await _sceneLoader.Load(SceneNames.MAIN_MENU);

        _uiRoot.HideLoadingScreen();
    }

    public override async UniTask Exit()
    {
        await base.Exit();

        await _assetProvider.ReleaseAssetsByLabel(AssetLabels.MainMenuState);
    }
}

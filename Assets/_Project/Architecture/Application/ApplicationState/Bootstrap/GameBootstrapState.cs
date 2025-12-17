using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class GameBootstrapState : BaseGameState
{
    private readonly SceneRootUI _sceneRootUI;
    private readonly IAssetProvider _assetProvider;
    private readonly IStaticDataService _staticDataService;
    private readonly ISavingService _savingService;

    public GameBootstrapState(GameStateMachine gameStateMachine, UIRootView uiRoot, SceneRootUI sceneRootUI,
        IAssetProvider assetProvider, IStaticDataService staticDataService, ISavingService savingService) : base(gameStateMachine, uiRoot)
    {
        _sceneRootUI = sceneRootUI;
        _staticDataService = staticDataService;
        _assetProvider = assetProvider;
        _savingService = savingService;
    }

    public override async UniTask Enter()
    {
        _uiRoot.ShowLoadingScreen();

        await base.Enter();

        _uiRoot.AttachSceneUI(_sceneRootUI.gameObject);

        await InitializeServices();

        _uiRoot.HideLoadingScreen();

        _gameStateMachine.Enter<GameMainMenuState>().Forget();
    }

    private async UniTask InitializeServices()
    {
        Debug.Log("initializing services in bootstrap state");
        await _assetProvider.InitializeAsync();
        _savingService.LoadProgress();
    }
}

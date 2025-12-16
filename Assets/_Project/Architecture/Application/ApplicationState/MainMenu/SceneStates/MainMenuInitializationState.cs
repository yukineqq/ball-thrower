using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class MainMenuInitializationState : BaseSceneState
{
    private readonly SceneRootUI _sceneUi;
    private readonly UIManager _manager;
    private readonly IInputService _inputService;

    public MainMenuInitializationState(GameStateMachine gameStateMachine, SceneStateMachine sceneStateMachine,
        UIRootView uiRoot, SceneRootUI sceneUi, UIManager manager, IInputService inputService) : base(gameStateMachine, sceneStateMachine, uiRoot)
    {
        _sceneUi = sceneUi;
        _manager = manager;
        _inputService = inputService;
    }

    public override async UniTask Enter()
    {
        await base.Enter();

        _inputService.Disable();

        InitializeUI();

        _uiRoot.HideLoadingScreen();

        _sceneStateMachine.Enter<MainMenuState>().Forget();
    }

    private void InitializeUI()
    {
        _sceneUi.Cleanup();

        _manager.CreateScreen<MainMenuScreenPresenter>();
    }
}

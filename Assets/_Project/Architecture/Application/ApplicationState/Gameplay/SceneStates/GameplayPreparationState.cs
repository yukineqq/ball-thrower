using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class GameplayPreparationState : BaseSceneState
{
    private readonly SceneRootUI _sceneUI;
    private readonly LevelService _levelService;
    private readonly IInputService _inputService;
    private readonly PlayingSessionHelper _sessionHelper;

    public GameplayPreparationState(GameStateMachine gameStateMachine, SceneStateMachine sceneStateMachine, UIRootView uiRoot, SceneRootUI sceneUI,
        LevelService levelService, IInputService inputService, PlayingSessionHelper sessionHelper) : base(gameStateMachine, sceneStateMachine, uiRoot)
    {
        _sceneUI = sceneUI;
        _levelService = levelService;
        _inputService = inputService;
        _sessionHelper = sessionHelper;
    }

    public override async UniTask Enter()
    {
        await base.Enter();

        _uiRoot.ShowLoadingScreen();

        _inputService.Disable();
        _inputService.ShowPointer(false);

        _sceneUI.CloseAllPopups();

        PrepareLevel();
        
        _uiRoot.HideLoadingScreen();

        _sceneStateMachine.Enter<GameplayRegularState>().Forget();
    }

    private void PrepareLevel()
    {
        _sessionHelper.ResetScore();

        _levelService.AutoSetupLevel();
    }
}

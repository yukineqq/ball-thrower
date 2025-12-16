using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

public sealed class GameplayPauseState : BaseSceneState
{
    private readonly IInputService _inputService;
    private readonly GameplayUIManager _uiManager;
    private readonly PlayingSessionHelper _sessionHelper;
    private readonly GolfTimer _timer;

    public GameplayPauseState(GameStateMachine gameStateMachine, SceneStateMachine sceneStateMachine, 
        UIRootView uiRoot, IInputService inputService,
        GameplayUIManager uiManager, PlayingSessionHelper sessionHelper, GolfTimer golfTimer) : base(gameStateMachine, sceneStateMachine, uiRoot)
    {
        _inputService = inputService;
        _uiManager = uiManager;
        _sessionHelper = sessionHelper;
        _timer = golfTimer;
    }

    public override async UniTask Enter()
    {
        await base.Enter();

        _inputService.Disable();
        _inputService.ShowPointer(false);

        _sessionHelper.SaveProgress();
        _timer.SetEnabled(false);
        Time.timeScale = 0f;

        _uiManager.CreatePopup<GameplayPausePopupPresenter>();
    }
}

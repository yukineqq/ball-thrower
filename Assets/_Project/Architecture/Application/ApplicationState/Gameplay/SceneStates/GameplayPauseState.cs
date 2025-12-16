using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

public sealed class GameplayPauseState : BaseSceneState
{
    private readonly GameplayUIManager _uiManager;
    private readonly GolfTimer _timer;
    public GameplayPauseState(GameStateMachine gameStateMachine, SceneStateMachine sceneStateMachine, 
        UIRootView uiRoot, GameplayUIManager uiManager, GolfTimer golfTimer) : base(gameStateMachine, sceneStateMachine, uiRoot)
    {
        _uiManager = uiManager;
        _timer = golfTimer;
    }

    public override async UniTask Enter()
    {
        await base.Enter();

        _timer.SetEnabled(false);
        Time.timeScale = 0f;

        _uiManager.CreatePopup<GameplayPausePopupPresenter>();
    }
}

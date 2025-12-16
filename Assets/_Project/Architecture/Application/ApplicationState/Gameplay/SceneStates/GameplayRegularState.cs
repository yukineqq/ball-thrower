using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class GameplayRegularState : BaseSceneState
{
    private readonly IInputService _inputService;
    private readonly GolfTimer _timer;

    public GameplayRegularState(GameStateMachine gameStateMachine, SceneStateMachine sceneStateMachine, UIRootView uiRoot,
        IInputService inputService, GolfTimer golfTimer) : base(gameStateMachine, sceneStateMachine, uiRoot)
    {
        _inputService = inputService;
        _timer = golfTimer;
    }

    public override async UniTask Enter()
    {
        await base.Enter();

        _timer.SetEnabled(true);
        Time.timeScale = 1f;

        _inputService.Enable();
        _inputService.ShowPointer(true);
    }
}

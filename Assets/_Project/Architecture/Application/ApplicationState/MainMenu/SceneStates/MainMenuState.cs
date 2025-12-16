using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class MainMenuState : BaseSceneState
{
    private readonly IInputService _inputService;

    public MainMenuState(GameStateMachine gameStateMachine, SceneStateMachine sceneStateMachine, UIRootView uiRoot, IInputService inputService) : base(gameStateMachine, sceneStateMachine, uiRoot)
    {
        _inputService = inputService;
    }

    public override async UniTask Enter()
    {
        await base.Enter();

        _inputService.Enable();
    }
}

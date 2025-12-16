using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class GameplayQuitState : BaseSceneState
{
    public GameplayQuitState(GameStateMachine gameStateMachine, SceneStateMachine sceneStateMachine, UIRootView uiRoot) : base(gameStateMachine, sceneStateMachine, uiRoot)
    {

    }

    public override async UniTask Enter()
    {
        await base.Enter();

        _uiRoot.ShowLoadingScreen();

        _gameStateMachine.Enter<GameMainMenuState>().Forget();
    }
}

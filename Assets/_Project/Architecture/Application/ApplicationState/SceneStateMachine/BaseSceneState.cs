using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class BaseSceneState : IExitableState
{
    protected readonly GameStateMachine _gameStateMachine;
    protected readonly SceneStateMachine _sceneStateMachine;
    protected readonly UIRootView _uiRoot;

    public BaseSceneState(GameStateMachine gameStateMachine, SceneStateMachine sceneStateMachine, UIRootView uiRoot)
    {
        _gameStateMachine = gameStateMachine;
        _sceneStateMachine = sceneStateMachine;
        _uiRoot = uiRoot;
    }

    public virtual async UniTask Enter()
    {
        Debug.Log($"{this.GetType().Name} enter");
    }

    public virtual UniTask Exit()
    {
        return default;
    }
}

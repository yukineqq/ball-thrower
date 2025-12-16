using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class BaseGameState : IGameState
{
    //protected readonly CancellationService _cancellationService;
    protected readonly GameStateMachine _gameStateMachine;
    protected readonly UIRootView _uiRoot;

    public BaseGameState(GameStateMachine gameStateMachine, UIRootView uiRoot)
    {
        _gameStateMachine = gameStateMachine;
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

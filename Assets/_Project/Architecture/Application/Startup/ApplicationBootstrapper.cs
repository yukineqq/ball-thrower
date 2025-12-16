using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public sealed class ApplicationBootstrapper : MonoBehaviour
{
    private GameStateMachine _gameStateMachine;
    private InfrastructureStatesFactory _statesFactory;

    [Inject]
    public void Construct(GameStateMachine gameStateMachine, InfrastructureStatesFactory statesFactory)
    {
        _gameStateMachine = gameStateMachine;
        _statesFactory = statesFactory;
    }

    private void Start()
    {
        Object.DontDestroyOnLoad(this.gameObject);

        Debug.Log("application bootstrapper");
        _gameStateMachine.RegisterState(_statesFactory.Create<GameBootstrapState>());
        _gameStateMachine.RegisterState(_statesFactory.Create<GameLoadingState>());
        _gameStateMachine.RegisterState(_statesFactory.Create<GameMainMenuState>());
        _gameStateMachine.RegisterState(_statesFactory.Create<GameGameplayState>());

        _gameStateMachine.Enter<GameBootstrapState>().Forget();
    }
}

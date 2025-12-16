using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public sealed class GameplayCoreBootstrapper : IInitializable
{
    private readonly SceneStateMachine _sceneStateMachine;
    private readonly InfrastructureStatesFactory _statesFactory;
    private readonly UIRootView _uiRoot;

    public GameplayCoreBootstrapper(SceneStateMachine sceneStateMachine, InfrastructureStatesFactory statesFactory, UIRootView uiRoot)
    {
         _sceneStateMachine = sceneStateMachine;
        _statesFactory = statesFactory;
        _uiRoot = uiRoot;
    }

    public void Initialize()
    {
        Debug.Log($"{this.GetType().Name} loaded");

        _uiRoot.ShowLoadingScreen();

        _sceneStateMachine.RegisterState(_statesFactory.Create<GameplayInitializationState>());
        _sceneStateMachine.RegisterState(_statesFactory.Create<GameplayPreparationState>());
        _sceneStateMachine.RegisterState(_statesFactory.Create<GameplayRegularState>());
        _sceneStateMachine.RegisterState(_statesFactory.Create<GameplayPauseState>());
        _sceneStateMachine.RegisterState(_statesFactory.Create<GameplayFinishState>());
        _sceneStateMachine.RegisterState(_statesFactory.Create<GameplayQuitState>());

        _sceneStateMachine.Enter<GameplayInitializationState>().Forget();
    }
}

using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public sealed class MainMenuCoreBootstrapper : IInitializable
{
    private readonly SceneStateMachine _sceneStateMachine;
    private readonly InfrastructureStatesFactory _statesFactory;
    private readonly UIRootView _uiRoot;

    public MainMenuCoreBootstrapper(SceneStateMachine sceneStateMachine, InfrastructureStatesFactory statesFactory, UIRootView uiRoot)
    {
        _sceneStateMachine = sceneStateMachine;
        _statesFactory = statesFactory;
        _uiRoot = uiRoot;
    }

    public void Initialize()
    {
        Debug.Log($"{this.GetType().Name} loaded");

        _uiRoot.ShowLoadingScreen();

        _sceneStateMachine.RegisterState(_statesFactory.Create<MainMenuInitializationState>());
        _sceneStateMachine.RegisterState(_statesFactory.Create<MainMenuState>());

        _sceneStateMachine.Enter<MainMenuInitializationState>().Forget();
    }
}

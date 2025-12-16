using UnityEngine;
using Zenject;

public sealed class MainMenuCoreInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindUIFactory();

        BindUIViewInstantiator();

        BindUIManager();

        BindMainMenuCore();
    }
    private void BindUIManager()
    {
        Container.Bind<UIManager>().To<MainMenuUIManager>().AsSingle();
    }

    private void BindUIFactory()
    {
        Container.BindInterfacesAndSelfTo<UIPresenterFactory>().AsSingle();
    }

    private void BindUIViewInstantiator()
    {
        Container.BindInterfacesAndSelfTo<UIViewInstantiator>().AsSingle();
    }

    private void BindMainMenuCore()
    {
        Container.BindInterfacesAndSelfTo<MainMenuCoreBootstrapper>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<InfrastructureStatesFactory>().AsSingle();

        Container.Bind<SceneStateMachine>().To<SceneStateMachine>().AsSingle();
    }
}

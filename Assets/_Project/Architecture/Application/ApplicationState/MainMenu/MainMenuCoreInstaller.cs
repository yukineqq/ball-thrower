using UnityEngine;
using Zenject;

public sealed class MainMenuCoreInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindAddressablePrefabInstantiator();

        BindPoolContainersManager();

        BindMainMenuFactories();

        BindPoolingService();

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

    private void BindPoolingService()
    {
        Container.BindInterfacesAndSelfTo<MainMenuPoolingService>().AsSingle();
    }

    private void BindMainMenuFactories()
    {
        Container.Bind<FactoryMonoBehaviourBase<WindowView>>().To<ScreenSpaceUIFactory>().AsSingle();
    }

    private void BindPoolContainersManager()
    {
        Container.BindInterfacesAndSelfTo<PoolParentContainersManager>().FromComponentInNewPrefabResource($"{ResourcesPaths.Infrastructure}ObjectPools").AsSingle();
    }

    private void BindAddressablePrefabInstantiator()
    {
        Container.BindInterfacesAndSelfTo<AddressablePrefabInstantiator>().AsSingle();
    }



    private void BindMainMenuCore()
    {
        Container.BindInterfacesAndSelfTo<MainMenuCoreBootstrapper>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<InfrastructureStatesFactory>().AsSingle();

        Container.Bind<SceneStateMachine>().To<SceneStateMachine>().AsSingle();
    }
}

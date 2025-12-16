using UnityEngine;
using Zenject;

public sealed class GameplayCoreInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindAddressablePrefabInstantiator();

        BindPoolContainersManager();

        BindGameplayFactories();

        BindPoolingService();

        BindUIFactory();

        BindUIViewInstantiator();

        BindUIManager();

        BindSpawnService();

        BindPlayerManager();

        BindLevelService();

        BindGolfTimer();

        BindGolfStateMachine();

        BindGameplayCore();
    }

    private void BindGolfStateMachine()
    {
        Container.BindInterfacesAndSelfTo<GolfStateMachine>().AsSingle();
    }

    private void BindGolfTimer()
    {
        Container.BindInterfacesAndSelfTo<GolfTimer>().AsSingle();
    }

    private void BindLevelService()
    {
        Container.BindInterfacesAndSelfTo<LevelService>().AsSingle();
    }

    private void BindPlayerManager()
    {
        Container.BindInterfacesAndSelfTo<PlayerManager>().AsSingle();
    }

    private void BindSpawnService()
    {
        Container.BindInterfacesAndSelfTo<SpawnService>().AsSingle();
    }

    private void BindUIManager()
    {
        Container.BindInterfacesAndSelfTo<GameplayUIManager>().AsSingle();
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
        Container.BindInterfacesAndSelfTo<GameplayPoolingService>().AsSingle();
    }

    private void BindGameplayFactories()
    {
        Container.Bind<FactoryMonoBehaviourBase<WindowView>>().To<ScreenSpaceUIFactory>().AsSingle();
        Container.Bind<FactoryMonoBehaviourBase<Entity>>().To<EntitiesFactory>().AsSingle();
    }

    private void BindPoolContainersManager()
    {
        Container.BindInterfacesAndSelfTo<PoolParentContainersManager>().FromComponentInNewPrefabResource($"{ResourcesPaths.Infrastructure}ObjectPools").AsSingle();
    }

    private void BindAddressablePrefabInstantiator()
    {
        Container.BindInterfacesAndSelfTo<AddressablePrefabInstantiator>().AsSingle();
    }

    private void BindGameplayCore()
    {
        Container.BindInterfacesAndSelfTo<GameplayCoreBootstrapper>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<InfrastructureStatesFactory>().AsSingle();

        Container.Bind<SceneStateMachine>().To<SceneStateMachine>().AsSingle();
    }
}

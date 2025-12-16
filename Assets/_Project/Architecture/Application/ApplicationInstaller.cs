using UnityEngine;
using Zenject;

public sealed class ApplicationInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindCancellationService();

        BindAssetProvider();

        BindAddressablePrefabInstaller();

        BindRootUI();

        BindSceneRootUI();

        BindCoroutinesService();

        BindSceneLoader();

        BindStatesFactory();

        BindGameStateMachine();

        BindStaticDataService();

        BindInputActions();

        BindInputHandler();

        BindInputService();

        BindSavingService();

        BindApplicationRunner();
    }

    private void BindSavingService()
    {
        Container.BindInterfacesAndSelfTo<SavingService>().AsSingle();
    }

    private void BindInputService()
    {
        Container.Bind<IInputService>().To<InputService>().AsSingle();
    }

    private void BindInputHandler()
    {
        Container.Bind<IInputHandler>().To<InputHandler>().AsSingle();
    }

    private void BindInputActions()
    {
        Container.Bind<PlayerInputActions>().To<PlayerInputActions>().AsSingle();
    }

    private void BindApplicationRunner()
    {
        var applicationRunner = Container.InstantiatePrefabResourceForComponent<ApplicationRunner>($"{ResourcesPaths.Infrastructure}ApplicationRunner");
        applicationRunner.transform.SetParent(null);

        Container.Bind<ApplicationRunner>().To<ApplicationRunner>().FromInstance(applicationRunner).AsSingle();
    }

    private void BindSceneLoader()
    {
        Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
    }

    private void BindStaticDataService()
    {
        Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
    }

    private void BindStatesFactory()
    {
        Container.BindInterfacesAndSelfTo<InfrastructureStatesFactory>().AsSingle();
    }

    private void BindCoroutinesService()
    {
        var coroutinesService = new GameObject(CoroutinesService.ServiceTitle).AddComponent<CoroutinesService>();
        Container.Bind<CoroutinesService>().FromInstance(coroutinesService).AsSingle();
        Object.DontDestroyOnLoad(coroutinesService.gameObject);
    }

    private void BindSceneRootUI()
    {
        Container.BindInterfacesAndSelfTo<SceneRootUI>().FromComponentInNewPrefabResource("UI/SceneRootUI").AsSingle();
    }

    private void BindRootUI()
    {
        var prefabUIRootView = Resources.Load<UIRootView>("UI/UIRoot");
        var uiRoot = Object.Instantiate(prefabUIRootView);
        Object.DontDestroyOnLoad(uiRoot.gameObject);
        Container.BindInterfacesAndSelfTo<UIRootView>().FromInstance(uiRoot).AsSingle().NonLazy();
    }

    private void BindAddressablePrefabInstaller()
    {
        Container.BindInterfacesAndSelfTo<AddressablePrefabInstantiator>().AsSingle();
    }

    private void BindAssetProvider()
    {
        Container.BindInterfacesAndSelfTo<AssetProvider>().AsSingle();
    }

    private void BindCancellationService()
    {
        Container.BindInterfacesAndSelfTo<CancellationService>().AsSingle();
    }

    private void BindGameStateMachine()
    {
        Container.Bind<GameStateMachine>().To<GameStateMachine>().AsSingle();
    }
}

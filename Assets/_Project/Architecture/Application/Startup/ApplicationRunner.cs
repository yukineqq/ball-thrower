using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public sealed class ApplicationRunner : MonoBehaviour
{
    private IInstantiator _instantiator;
    private ISceneLoader _sceneLoader;

    [Inject]
    public void Construct(IInstantiator instantiator, ISceneLoader sceneLoader)
    {
        _instantiator = instantiator;
        _sceneLoader = sceneLoader;

        Debug.Log("application runner");
    }

    private void Awake()
    {
        Object.DontDestroyOnLoad(this.gameObject);

        var bootstrapper = FindFirstObjectByType<ApplicationBootstrapper>();

        if (bootstrapper == null)
        {
            CreateApplicationBootstrapper();
        }

        RunApplication().Forget();
    }

    private async UniTask RunApplication()
    {
        await _sceneLoader.Load(SceneNames.INITIALIZATION);
    }

    private ApplicationBootstrapper CreateApplicationBootstrapper()
    {
        var bootstrapper = _instantiator.InstantiatePrefabResourceForComponent<ApplicationBootstrapper>($"{ResourcesPaths.Infrastructure}ApplicationBootstrapper");
        bootstrapper.transform.SetParent(null);
        return bootstrapper;
    }
}

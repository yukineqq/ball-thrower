using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;

public sealed class SceneLoader : ISceneLoader
{
    private readonly UIRootView _uiRoot;

    private readonly List<AsyncOperationHandle<SceneInstance>> _additiveSceneHandles = new List<AsyncOperationHandle<SceneInstance>>();

    public string CurrentSceneName => SceneManager.GetActiveScene().name;

    public event Action EmergencyTransition;

    public SceneLoader(UIRootView uiRoot)
    {
        _uiRoot = uiRoot;
    }

    public async UniTask Load(string targetScene)
    {
        CancellationToken cancellationToken = Application.exitCancellationToken;

        try
        {
            switch (targetScene)
            {
                case SceneNames.INITIALIZATION:
                    await Bootstrap(cancellationToken);
                    break;
                case SceneNames.LOADING:
                    await ToLoadingScene(cancellationToken);
                    break;
                case SceneNames.MAIN_MENU:
                    await LoadAndStartMainMenu(cancellationToken);
                    break;
                case SceneNames.GAMEPLAYCORE:
                    await LoadAndStartGameplay(cancellationToken);
                    break;
                default:
                    Debug.LogWarning($"scene '{targetScene}' is not exists: safe auto redirection to initialization scene");
                    EmergencyTransition?.Invoke();
                    await Bootstrap(cancellationToken);
                    break;
            }
        }
        catch (OperationCanceledException exception)
        {
            Debug.Log($"error while loading core scene: {exception.Message}");
        }
    }

    public Scene GetScene(string sceneName)
    {
        return SceneManager.GetSceneByName(sceneName);
    }

    public Scene GetScene(int sceneIndex)
    {
        return SceneManager.GetSceneByBuildIndex(sceneIndex);
    }

    public async UniTask<Scene> LoadAdditiveFromBuild(string additiveScene, bool activationOnLoad = true)
    {
        CancellationToken cancellationToken = Application.exitCancellationToken;

        await UniTask.Yield(cancellationToken);

        var asyncResourcesCleanup = Resources.UnloadUnusedAssets();
        while (!asyncResourcesCleanup.isDone)
        {
            await UniTask.Yield(cancellationToken);
        }

        Debug.Log($"\t\t\t\t\t\t\t\t loading additive scene {additiveScene}");
        var asyncOperation = SceneManager.LoadSceneAsync(additiveScene, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = activationOnLoad;

        while (asyncOperation.progress < 0.9f)
        {
            await UniTask.Yield(cancellationToken);
        }

        if (!activationOnLoad)
        {
            return GetScene(additiveScene);
        }

        while (!asyncOperation.isDone)
        {
            await UniTask.Yield(cancellationToken);
        }

        asyncOperation.allowSceneActivation = true;
        return GetScene(additiveScene);
    }

    public async UniTask UnloadAdditiveFromBuild(string additiveScene)
    {
        CancellationToken cancellationToken = Application.exitCancellationToken;

        Debug.Log($"\t\t\t\t\t\t\t\t unloading additive scene {additiveScene}");
        var asyncOperation = SceneManager.UnloadSceneAsync(additiveScene);
        while (!asyncOperation.isDone)
        {
            await UniTask.Yield(cancellationToken);
        }
    }

    public async UniTask<Scene> LoadAdditiveFromBundle(string additiveScene, bool activationOnLoad = true)
    {
        CancellationToken cancellationToken = Application.exitCancellationToken;

        await UniTask.Yield(cancellationToken);

        var asyncResourcesCleanup = Resources.UnloadUnusedAssets();
        while (!asyncResourcesCleanup.isDone)
        {
            await UniTask.Yield(cancellationToken);
        }

        Debug.Log($"\t\t\t\t\t\t\t\t loading additive scene {additiveScene}");
        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(additiveScene, LoadSceneMode.Additive, false);

        cancellationToken.ThrowIfCancellationRequested();

        await handle.ToUniTask().AttachExternalCancellation(cancellationToken);

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Addressables.Release(handle);
            throw new Exception($"loading additive scene {additiveScene} failed");
        }

        cancellationToken.ThrowIfCancellationRequested();

        if (activationOnLoad)
        {
            AsyncOperation asyncOperation = handle.Result.ActivateAsync();

            while (!asyncOperation.isDone)
            {
                await UniTask.Yield(cancellationToken);
            }
        }

        _additiveSceneHandles.Add(handle); //Debug.Log($"additive scene handles count: {_additiveSceneHandles.Count} / scenes loaded: {SceneManager.loadedSceneCount}");

        return handle.Result.Scene;
    }

    public async UniTask UnloadAdditiveFromBundle(string additiveScene)
    {
        CancellationToken cancellationToken = Application.exitCancellationToken;

        Debug.Log($"\t\t\t\t\t\t\t\t unloading additive scene {additiveScene}");

        if (_additiveSceneHandles.Count <= 0)
        {
            return;
        }

        List<UniTask<SceneInstance>> tasks = new List<UniTask<SceneInstance>>(_additiveSceneHandles.Count);

        foreach (var additiveHandle in _additiveSceneHandles)
        {
            tasks.Add(Addressables.UnloadSceneAsync(additiveHandle).ToUniTask().AttachExternalCancellation(cancellationToken));
        }

        _additiveSceneHandles.Clear();

        await UniTask.WhenAll(tasks).AttachExternalCancellation(cancellationToken); //Debug.Log($"additive scene handles count: {_additiveSceneHandles.Count} / scenes loaded: {SceneManager.loadedSceneCount}");
    }

    private async UniTask Bootstrap(CancellationToken cancellationToken)
    {
        await UniTask.Yield(cancellationToken);
        await LoadSceneFromBuild(SceneNames.INITIALIZATION, cancellationToken);
    }

    public async UniTask ToLoadingScene(CancellationToken cancellationToken)
    {
        await LoadSceneFromBundle(SceneNames.LOADING, cancellationToken);
    }

    private async UniTask LoadAndStartMainMenu(CancellationToken cancellationToken)
    {
        await LoadSceneFromBundle(SceneNames.MAIN_MENU, cancellationToken);
    }

    private async UniTask LoadAndStartGameplay(CancellationToken cancellationToken)
    {
        _uiRoot.ShowLoadingScreen();

        var asyncOperation = Resources.UnloadUnusedAssets();

        while (!asyncOperation.isDone)
        {
            await UniTask.Yield(cancellationToken);
        }

        await LoadSceneFromBundle(SceneNames.GAMEPLAYCORE, cancellationToken);

        _uiRoot.HideLoadingScreen();
    }

    private async UniTask LoadSceneFromBuild(string sceneName, CancellationToken cancellationToken)
    {
        Debug.Log($"\t\t\t\t\t\t\t\t transition from {SceneManager.GetActiveScene().name} to {sceneName}");

        if (cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return;
        }

        var asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone && !cancellationToken.IsCancellationRequested)
        {
            await UniTask.Yield(cancellationToken);
        }

        if (cancellationToken.IsCancellationRequested)
        {
            asyncOperation.allowSceneActivation = false;
            return;
        }
    }

    private async UniTask LoadSceneFromBundle(string sceneName, CancellationToken cancellationToken)
    {
        Debug.Log($"\t\t\t\t\t\t\t\t transition from {SceneManager.GetActiveScene().name} to {sceneName}");

        if (cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return;
        }

        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single, false);

        while (!handle.IsDone && !cancellationToken.IsCancellationRequested)
        {
            await UniTask.Yield(cancellationToken);
        }

        if (!cancellationToken.IsCancellationRequested)
        {
            await handle.Result.ActivateAsync().ToUniTask().AttachExternalCancellation(cancellationToken);
            return;
        }
    }
}

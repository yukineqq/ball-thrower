using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Cysharp.Threading.Tasks;

public sealed class AssetProvider : IAssetProvider, IDisposable
{
    private readonly CancellationService _cancellationService;
    private readonly Dictionary<string, AsyncOperationHandle> _assetRequestsMap = new Dictionary<string, AsyncOperationHandle>();

    public int LoadedAssetsCount => _assetRequestsMap.Count;

    public AssetProvider(CancellationService cancellationService)
    {
        _cancellationService = cancellationService;
    }

    public async UniTask InitializeAsync()
    {
        await Addressables.InitializeAsync().ToUniTask().AttachExternalCancellation(_cancellationService.ApplicationExitCancellationToken);
    }

    public void Dispose()
    {
        Cleanup();
    }

    public bool GetAsset<TAsset>(string key, out TAsset asset) where TAsset : class
    {
        if (_assetRequestsMap.TryGetValue(key, out AsyncOperationHandle handle))
        {
            asset = handle.Result as TAsset;
            return true;
        }

        asset = null;
        return false;
    }

    public async UniTask<TAsset> Load<TAsset>(string key) where TAsset : class
    {
        _cancellationService.ApplicationExitCancellationToken.ThrowIfCancellationRequested();

        if (!_assetRequestsMap.TryGetValue(key, out AsyncOperationHandle handle))
        {
            handle = Addressables.LoadAssetAsync<TAsset>(key);
            _assetRequestsMap.Add(key, handle);
        }

        await handle.ToUniTask().AttachExternalCancellation(_cancellationService.ApplicationExitCancellationToken);

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Addressables.Release(handle);
            throw new Exception($"failed to load addressable asset at {key} of type {typeof(TAsset).Name}");
        }

        return handle.Result as TAsset;
    }

    public async UniTask<TAsset> Load<TAsset>(AssetReference assetReference) where TAsset : class
    {
        _cancellationService.ApplicationExitCancellationToken.ThrowIfCancellationRequested();

        return await Load<TAsset>(assetReference.AssetGUID);
    }

    public async UniTask<List<string>> GetAssetsListByLabel<TAsset>(string label) where TAsset : class
    {
        _cancellationService.ApplicationExitCancellationToken.ThrowIfCancellationRequested();

        return await GetAssetsListByLabel(label, typeof(TAsset));
    }

    public async UniTask<List<string>> GetAssetsListByLabel(string label, Type type = null)
    {
        _cancellationService.ApplicationExitCancellationToken.ThrowIfCancellationRequested();

        AsyncOperationHandle<IList<IResourceLocation>> handle = Addressables.LoadResourceLocationsAsync(label, type);

        IList<IResourceLocation> locations = await handle.ToUniTask().AttachExternalCancellation(_cancellationService.ApplicationExitCancellationToken);

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Addressables.Release(handle);
            throw new Exception($"failed to load addressable assets with label: {label}");
        }

        List<string> assetKeys = new List<string>(locations.Count);

        foreach (IResourceLocation location in locations)
        {
            assetKeys.Add(location.PrimaryKey);
        }

        Addressables.Release(handle);
        return assetKeys;
    }

    public async UniTask<TAsset[]> LoadAll<TAsset>(List<string> keys) where TAsset : class
    {
        _cancellationService.ApplicationExitCancellationToken.ThrowIfCancellationRequested();

        List<UniTask<TAsset>> tasks = new List<UniTask<TAsset>>(keys.Count);

        foreach (string key in keys)
        {
            tasks.Add(Load<TAsset>(key));
        }

        return await UniTask.WhenAll(tasks).AttachExternalCancellation(_cancellationService.ApplicationExitCancellationToken);
    }

    public async UniTask WarmupAssetsByLabel(string label)
    {
        _cancellationService.ApplicationExitCancellationToken.ThrowIfCancellationRequested();

        List<string> assetsList = await GetAssetsListByLabel(label);
        await LoadAll<object>(assetsList);

        Debug.Log($"loaded assets count: {LoadedAssetsCount}".ToUpper());
    }

    public async UniTask ReleaseAssetsByLabel(string label)
    {
        _cancellationService.ApplicationExitCancellationToken.ThrowIfCancellationRequested();

        List<string> assetsList = await GetAssetsListByLabel(label);

        foreach (string assetKey in assetsList)
        {
            ReleaseAsset(assetKey);
        }

        Debug.Log($"loaded assets count: {LoadedAssetsCount}".ToUpper());
    }

    public void ReleaseAsset(string assetKey)
    {
        if (_assetRequestsMap.TryGetValue(assetKey, out AsyncOperationHandle handle))
        {
            Addressables.Release(handle);
            _assetRequestsMap.Remove(assetKey);
        }
    }

    public void ReleaseAsset(AssetReference assetReference)
    {
        ReleaseAsset(assetReference.AssetGUID);
    }

    public void Cleanup()
    {
        foreach (KeyValuePair<string, AsyncOperationHandle> assetRequest in _assetRequestsMap)
        {
            Addressables.Release(assetRequest.Value);
        }

        _assetRequestsMap.Clear();
    }
}

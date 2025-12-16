using System;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public interface IAssetProvider
{
    public int LoadedAssetsCount { get; }

    public UniTask InitializeAsync();
    public bool GetAsset<TAsset>(string key, out TAsset asset) where TAsset : class;
    public UniTask<TAsset> Load<TAsset>(string key) where TAsset : class;
    public UniTask<TAsset> Load<TAsset>(AssetReference assetReference) where TAsset : class;
    public UniTask<List<string>> GetAssetsListByLabel<TAsset>(string label) where TAsset : class;
    public UniTask<List<string>> GetAssetsListByLabel(string label, Type type = null);
    public UniTask<TAsset[]> LoadAll<TAsset>(List<string> keys) where TAsset : class;
    public UniTask WarmupAssetsByLabel(string label);
    public UniTask ReleaseAssetsByLabel(string label);
    public void ReleaseAsset(string assetKey);
    public void ReleaseAsset(AssetReference assetReference);
    public void Cleanup();
}

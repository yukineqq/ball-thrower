using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public sealed class StaticDataService : IStaticDataService
{
    private readonly IInstantiator _instantiator;
    private readonly IAssetProvider _assetProvider;

    private readonly Dictionary<Type, object> _singleConfigs = new Dictionary<Type, object>();
    
    public StaticDataService(IInstantiator instantiator, IAssetProvider assetProvider)
    {
        _instantiator = instantiator;
        _assetProvider = assetProvider;

        Debug.Log("static data service created");
    }

    public async UniTask InitializeAsync()
    {
        List<UniTask> tasks = new List<UniTask>();

        tasks.Add(UniTask.WaitForEndOfFrame());

        await UniTask.WhenAll(tasks);

        Debug.Log("static data service initialized");
    }

    private async UniTask<List<string>> GetConfigsKeys<TConfig>()
    {
        return await _assetProvider.GetAssetsListByLabel(AssetLabels.Config);
    }

    private async UniTask<TConfig[]> GetConfigs<TConfig>() where TConfig : class
    {
        List<string> keys = await GetConfigsKeys<TConfig>();
        return await _assetProvider.LoadAll<TConfig>(keys);
    }

    private async UniTask<TConfig> LoadSingleConfig<TConfig>() where TConfig : class
    {
        TConfig[] configs = await GetConfigs<TConfig>();

        TConfig config;

        if (configs.Length > 0)
        {
            config = configs[0];
            _singleConfigs.Add(typeof(TConfig), config);

            return config;
        }

        throw new Exception($"can't find config of type {typeof(TConfig).Name}");
    }

    public async UniTask<TConfig> GetSingleConfig<TConfig>() where TConfig : class
    {
        if (_singleConfigs.TryGetValue(typeof(TConfig), out object config))
        {
            return config as TConfig;
        }

        return await LoadSingleConfig<TConfig>();
    }

    public async UniTask<TConfig> LoadSingleConfigByPath<TConfig>(string path) where TConfig : ScriptableObject
    {
        path = $"Configs/{path}";
        return await GetCachedOrInstantiateConfigByType<TConfig>(_singleConfigs, typeof(TConfig), path, false);
    }

    private async UniTask<TConfig> GetCachedOrInstantiateConfigByKey<TConfig>(Dictionary<string, object> configMap, string key, bool fromBundle = true) where TConfig : ScriptableObject
    {
        if (configMap.TryGetValue(key, out object cachedConfig))
        {
            return (TConfig)cachedConfig;
        }

        TConfig config;

        if (fromBundle)
        {
            TConfig asset = await _assetProvider.Load<TConfig>(key);
            config = ScriptableObject.Instantiate<TConfig>(asset);
        }
        else
        {
            config = _instantiator.InstantiateScriptableObjectResource<TConfig>(key);
        }

        configMap.Add(key, config);

        return config;
    }

    public TConfig GetResourcesSingleConfigByPath<TConfig>(string configAddress) where TConfig : ScriptableObject
    {
        if (_singleConfigs.TryGetValue(typeof(TConfig), out object cachedConfig))
        {
            return (TConfig)cachedConfig;
        }

        TConfig config;
        config = _instantiator.InstantiateScriptableObjectResource<TConfig>(configAddress);
        _singleConfigs.Add(typeof(TConfig), config);

        return config;
    }

    private async UniTask<TConfig> GetCachedOrInstantiateConfigByType<TConfig>(Dictionary<Type, object> configMap, Type key, string configAddress, bool fromBundle = true) where TConfig : ScriptableObject
    {
        if (configMap.TryGetValue(key, out object cachedConfig))
        {
            return (TConfig)cachedConfig;
        }

        TConfig config;

        if (fromBundle)
        {
            TConfig asset = await _assetProvider.Load<TConfig>(configAddress);
            config = ScriptableObject.Instantiate<TConfig>(asset);
        }
        else
        {
            config = _instantiator.InstantiateScriptableObjectResource<TConfig>(configAddress);
        }

        configMap.Add(key, config);

        return config;
    }
}

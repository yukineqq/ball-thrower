using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;
using System.Threading;

public sealed class AddressablePrefabInstantiator
{
    private readonly DiContainer _container;
    private readonly IAssetProvider _assetProvider;

    public AddressablePrefabInstantiator(DiContainer container, IAssetProvider assetProvider)
    {
        _container = container;
        _assetProvider = assetProvider;
    }

    public TPrefabComponent InstantiatePreloadedAddressablePrefabForComponent<TPrefabComponent>(string key, Transform parentTransform) where TPrefabComponent : Component
    {
        if (!_assetProvider.GetAsset<GameObject>(key, out GameObject prefab))
        {
            throw new System.Exception($"asset at {key} is not loaded");
        }

        TPrefabComponent instance = _container.InstantiatePrefabForComponent<TPrefabComponent>(prefab, parentTransform);

        return instance;
    }

    public TPrefabComponent InstantiatePreloadedAddressablePrefabForComponent<TPrefabComponent>(string key, Vector3 position = default, Quaternion rotation = default, Transform parentTransform = null) where TPrefabComponent : Component
    {
        if (!_assetProvider.GetAsset<GameObject>(key, out GameObject prefab))
        {
            throw new System.Exception($"asset at {key} is not loaded");
        }

        TPrefabComponent instance = _container.InstantiatePrefabForComponent<TPrefabComponent>(prefab, position, rotation, parentTransform);

        return instance;
    }

    public GameObject InstantiatePreloadedAddressablePrefabForGameObject(string key, Vector3 position = default, Quaternion rotation = default, Transform parentTransform = null)
    {
        if (!_assetProvider.GetAsset<GameObject>(key, out GameObject prefab))
        {
            throw new System.Exception($"asset at {key} is not loaded");
        }

        GameObject instance = _container.InstantiatePrefab(prefab);

        return instance;
    }

    public TScriptableObject InstantiatePreloadedAddressableScriptableObject<TScriptableObject>(string key) where TScriptableObject : ScriptableObject
    {
        if (!_assetProvider.GetAsset<ScriptableObject>(key, out ScriptableObject scriptableObjectAsset))
        {
            throw new System.Exception($"scriptable object asset at {key} is not loaded");
        }

        TScriptableObject Instance = ScriptableObject.Instantiate(scriptableObjectAsset as TScriptableObject);

        return Instance;
    }

    public async UniTask<TPrefabComponent> InstantiateAddressablePrefabForComponentAsync<TPrefabComponent>(string key, Vector3 position = default, Quaternion rotation = default, Transform parentTransform = null, bool releaseImmediately = false, CancellationToken cancellationToken = default) where TPrefabComponent : Component
    {
        TPrefabComponent prefab = await _assetProvider.Load<TPrefabComponent>(key);

        TPrefabComponent instance = _container.InstantiatePrefabForComponent<TPrefabComponent>(prefab, position, rotation, parentTransform);

        if (releaseImmediately)
        {
            _assetProvider.ReleaseAsset(key);
        }

        return instance;
    }

    public async UniTask<GameObject> InstantiateAddressablePrefabForGameObjectAsync(string key, Vector3 position = default, Quaternion rotation = default, Transform parentTransform = null, bool releaseImmediately = false, CancellationToken cancellationToken = default)
    {
        GameObject prefab = await _assetProvider.Load<GameObject>(key);

        GameObject instance = _container.InstantiatePrefab(prefab);

        if (releaseImmediately)
        {
            _assetProvider.ReleaseAsset(key);
        }

        return instance;
    }

    public async UniTask<TScriptableObject> InstantiateAddressableScriptableObjectAsync<TScriptableObject>(string key, bool releaseImmediately = false, CancellationToken cancellationToken = default) where TScriptableObject : ScriptableObject
    {
        TScriptableObject scriptableObjectAsset = await _assetProvider.Load<TScriptableObject>(key);

        TScriptableObject Instance = ScriptableObject.Instantiate(scriptableObjectAsset);

        if (releaseImmediately)
        {
            _assetProvider.ReleaseAsset(key);
        }

        return Instance;
    }
}

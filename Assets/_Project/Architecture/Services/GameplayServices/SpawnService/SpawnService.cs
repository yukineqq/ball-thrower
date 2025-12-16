using UnityEngine;

public sealed class SpawnService : ISpawnService
{
    private readonly GameplayPoolingService _poolingService;
    private readonly AddressablePrefabInstantiator _addressablePrefabInstantiator;

    public SpawnService(GameplayPoolingService poolingService, AddressablePrefabInstantiator addressablePrefabInstantiator)
    {
        _poolingService = poolingService;
        _addressablePrefabInstantiator = addressablePrefabInstantiator;
    }

    public T Spawn<T>(string key, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : MonoBehaviour
    {
        return _addressablePrefabInstantiator.InstantiatePreloadedAddressablePrefabForComponent<T>(key, position, rotation);
    }

    public T SpawnFromPool<T>(Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : MonoBehaviour
    {
        T obj = _poolingService.GetPool<EntitiesObjectPool>().Get<T>(position, rotation, parent);

        obj.gameObject.SetActive(true);

        if (obj is IPoolableObject poolable)
        {
            poolable.OnAcquire();
        }

        return obj;
    }

    public void ReleaseObject(MonoBehaviour instance)
    {
        _poolingService.GetPool<EntitiesObjectPool>().Release(instance);
        return;
    }

    public void SetObjectTracking(MonoBehaviour instance, bool value)
    {
        _poolingService.GetPool<EntitiesObjectPool>().SetObjectTracking(instance, value);
        return;
    }
}

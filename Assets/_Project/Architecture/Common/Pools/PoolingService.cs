using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class PoolingService : IInitializable, IDisposable
{
    private readonly IInstantiator _instantiator;
    private readonly PoolingServiceConfig _config;
    private readonly Dictionary<Type, IMonoBehaviourObjectPool> _poolsMap = new Dictionary<Type, IMonoBehaviourObjectPool>();

    public PoolingService(IInstantiator instantiator, IStaticDataService staticDataService)
    {
        _instantiator = instantiator;
        _config = staticDataService.GetResourcesSingleConfigByPath<PoolingServiceConfig>("Configs/Infrastructure/PoolingServiceConfig");
    }

    public abstract void Initialize();

    public void Dispose()
    {
        Cleanup();
    }

    public void Cleanup()
    {
        foreach (var element in _poolsMap)
        {
            element.Value.Cleanup();
        }
    }

    public TObjectPool GetPool<TObjectPool>() where TObjectPool : class, IMonoBehaviourObjectPool
    {
        if (_poolsMap.TryGetValue(typeof(TObjectPool), out IMonoBehaviourObjectPool pool))
        {
            return pool as TObjectPool;
        }

        return null;
    }

    public bool RegisterPool<TObjectPool>() where TObjectPool : class, IMonoBehaviourObjectPool
    {
        if (_poolsMap.TryGetValue(typeof(TObjectPool), out IMonoBehaviourObjectPool pool))
        {
            return false;
        }

        pool = _instantiator.Instantiate<TObjectPool>();
        pool.ChangeCapacity(_config.InitialPoolCapacity);

        _poolsMap.Add(typeof(TObjectPool), pool);
        return true;
    }

    public bool DestroyPool<TObjectPool>() where TObjectPool : class, IMonoBehaviourObjectPool
    {
        bool result = _poolsMap.Remove(typeof(TObjectPool), out IMonoBehaviourObjectPool pool);

        if (result)
        {
            pool.Cleanup();
        }

        return result;
    }
}

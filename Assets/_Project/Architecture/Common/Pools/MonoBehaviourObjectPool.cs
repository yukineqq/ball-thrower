using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class MonoBehaviourObjectPool<T, TFactory> : IMonoBehaviourObjectPool, IDisposable where T : MonoBehaviour where TFactory : FactoryMonoBehaviourBase<T>
{
    protected readonly PoolParentContainersManager _containersManager;
    protected readonly TFactory _factory;
    protected readonly Transform _parentContainer;
    protected readonly List<T> _pooledObjects = new List<T>();
    protected readonly List<T> _activeObjects = new List<T>();

    public abstract string RootFolder { get; }
    public int Capacity { get; private set; } = 10;

    public MonoBehaviourObjectPool(PoolParentContainersManager containersManager, TFactory factory)
    {
        _containersManager = containersManager;
        _factory = factory;
        _parentContainer = containersManager.GetContainer<T>();
    }

    public void Dispose()
    {
        Cleanup();

        _containersManager.DestroyContainer<T>();
    }

    public void ChangeCapacity(int capacity = 10)
    {
        Capacity = capacity;

        int objectsOverTargetCapacityCount = _pooledObjects.Count - capacity;

        for (int i = 0; i < objectsOverTargetCapacityCount; i++)
        {
            var objectToDestroy = _pooledObjects[_pooledObjects.Count - 1];
            _pooledObjects.Remove(objectToDestroy);
            Destroy(objectToDestroy);
        }
    }

    public TSpecified Get<TSpecified>() where TSpecified : MonoBehaviour
    {
        return GetInternal<TSpecified>(default, default, null, true);
    }

    public TSpecified Get<TSpecified>(Vector3 position = default, Quaternion rotation = default, Transform parent = null, bool worldsPositionsStays = true) where TSpecified : MonoBehaviour
    {
        return GetInternal<TSpecified>(position, rotation, parent, worldsPositionsStays);
    }

    private TSpecified GetInternal<TSpecified>(Vector3 position, Quaternion rotation, Transform parent, bool worldsPositionsStays = true) where TSpecified : MonoBehaviour
    {
        TSpecified instanceToReturn = null;

        if (_pooledObjects.Count <= 0)
        {
            instanceToReturn = Create<TSpecified>(position, rotation, parent);
            OnInstanceAcquire<TSpecified>(instanceToReturn);

            return instanceToReturn;
        }

        int i = 0;
        while (instanceToReturn == null && i < _pooledObjects.Count)
        {
            if (_pooledObjects[i] is TSpecified instanceOfRequestedType)
            {
                instanceToReturn = instanceOfRequestedType;
            }

            i++;
        }

        if (instanceToReturn == null)
        {
            instanceToReturn = Create<TSpecified>(position, rotation, parent);
        }

        instanceToReturn.transform.SetParent(parent, worldsPositionsStays);
        instanceToReturn.transform.position = position;
        instanceToReturn.transform.rotation = rotation;

        OnInstanceAcquire<TSpecified>(instanceToReturn);
        return instanceToReturn;
    }

    public void Release(MonoBehaviour objectToRelease)
    {
        T instance = objectToRelease as T;

        if (instance == null || instance.gameObject == null)
        {
            return;
        }

        if (_pooledObjects.Contains(instance))
        {
            Debug.Log($"object {instance.GetType().Name} is already released to pool");
            return;
        }

        if (_pooledObjects.Count >= Capacity)
        {
            Destroy(instance);
            return;
        }

        OnInstanceRelease<T>(instance);
    }

    public bool PreCreate<TSpecified>(int amount, out int notInstantiatedObjectsAmount) where TSpecified : MonoBehaviour
    {
        notInstantiatedObjectsAmount = _pooledObjects.Count + amount - Capacity;

        if (notInstantiatedObjectsAmount > 0)
        {
            amount -= notInstantiatedObjectsAmount;
        }
        else
        {
            notInstantiatedObjectsAmount = 0;
        }

        for (int i = 0; i < amount; i++)
        {
            Create<TSpecified>();
        }

        return notInstantiatedObjectsAmount > 0;
    }

    protected virtual TSpecified Create<TSpecified>(Vector3 position = default, Quaternion rotation = default, Transform parent = null) where TSpecified : MonoBehaviour
    {
        return _factory.Create<TSpecified>(position, rotation, parent);
    }

    protected virtual void OnInstanceAcquire<TSpecified>(TSpecified instanceToAcquire) where TSpecified : MonoBehaviour
    {
        if (instanceToAcquire is not T instance)
        {
            return;
        }

        instance.transform.SetParent(null);

        _pooledObjects.Remove(instance);
        _activeObjects.Add(instance);

        if (instance is IPoolableObject poolable)
        {
            poolable.OnAcquire();
        }
    }

    protected virtual void OnInstanceRelease<TSpecified>(TSpecified instanceToRelease) where TSpecified : MonoBehaviour
    {
        if (instanceToRelease is not T instance)
        {
            return;
        }

        instance.gameObject.SetActive(false);

        if (_parentContainer != null && _parentContainer.transform != null)
        {
            instance.transform.SetParent(_parentContainer.transform);
        }

        instance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        _activeObjects.Remove(instance);
        _pooledObjects.Add(instance);

        if (instance is IPoolableObject poolable)
        {
            poolable.OnRelease();
        }
    }

    public void SetObjectTracking(MonoBehaviour instance, bool value)
    {
        T instanceTyped = instance as T;
        bool contains = _pooledObjects.Contains(instanceTyped) || _activeObjects.Contains(instanceTyped);

        if (value)
        {
            if (!contains)
            {
                switch (instanceTyped.gameObject.activeSelf)
                {
                    case true:
                        _activeObjects.Add(instanceTyped);
                        break;
                    case false:
                        _pooledObjects.Add(instanceTyped);
                        instanceTyped.transform.SetParent(_parentContainer);
                        break;
                }
            }

            return;
        }

        if (instanceTyped.gameObject.activeSelf)
        {
            _activeObjects.Remove(instanceTyped);
        }
    }

    public void ReleaseAll()
    {
        List<T> instancesToRelease = new List<T>(_activeObjects);

        foreach (var instance in instancesToRelease)
        {
            Release(instance);
        }

        _activeObjects.Clear();
    }

    public void Cleanup()
    {
        Clear(true);
        Clear(false);
    }

    public void Clear(bool isPooled = true)
    {
        List<T> objects = isPooled ? _pooledObjects : _activeObjects;

        foreach (T obj in objects)
        {
            Destroy(obj);
        }

        objects.Clear();
    }

    protected void Destroy(T objectToDestroy)
    {
        if (objectToDestroy != null && objectToDestroy.gameObject != null)
        {
            GameObject.Destroy(objectToDestroy.gameObject);
        }
    }
}

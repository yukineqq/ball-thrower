using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class PoolParentContainersManager : MonoBehaviour
{
    private readonly Dictionary<Type, Transform> _poolParentContainersMap = new Dictionary<Type, Transform>();

    public Transform GetContainer<T>() where T : MonoBehaviour
    {
        if (_poolParentContainersMap.TryGetValue(typeof(T), out Transform parentContainer))
        {
            return parentContainer;
        }

        parentContainer = CreateContainer<T>();

        _poolParentContainersMap.Add(typeof(T), parentContainer.transform);

        return parentContainer;
    }

    public bool DestroyContainer<T>() where T : MonoBehaviour
    {
        if (_poolParentContainersMap.TryGetValue(typeof(T), out Transform parentContainer))
        {
            GameObject.Destroy(parentContainer.gameObject);
            return true;
        }

        return false;
    }

    private Transform CreateContainer<T>() where T : MonoBehaviour
    {
        Transform parentContainer = new GameObject($"{typeof(T).Name}PoolContainer").transform;

        parentContainer.transform.SetParent(this.transform);

        return parentContainer;
    }
}

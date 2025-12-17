using UnityEngine;
using Zenject;

public sealed class UIObjectPool : MonoBehaviourObjectPool<WindowView, FactoryMonoBehaviourBase<WindowView>>
{
    public override string RootFolder => "UI";

    public UIObjectPool(PoolParentContainersManager containersManager, FactoryMonoBehaviourBase<WindowView> factory) : base(containersManager, factory)
    {

    }

    protected override void OnInstanceAcquire<TSpecified>(TSpecified instanceToAcquire)
    {
        if (instanceToAcquire is not WindowView instance)
        {
            return;
        }

        instance.transform.SetParent(null, false);

        _pooledObjects.Remove(instance);
        _activeObjects.Add(instance);

        if (instance is IPoolableObject poolable)
        {
            poolable.OnAcquire();
        }
    }

    protected override void OnInstanceRelease<TSpecified>(TSpecified instanceToRelease)
    {
        if (instanceToRelease is not WindowView instance)
        {
            return;
        }

        instance.gameObject.SetActive(false);

        if (_parentContainer != null && _parentContainer.transform != null)
        {
            instance.transform.SetParent(_parentContainer.transform, false);
        }

        _activeObjects.Remove(instance);
        _pooledObjects.Add(instance);

        if (instance is IPoolableObject poolable)
        {
            poolable.OnRelease();
        }
    }
}

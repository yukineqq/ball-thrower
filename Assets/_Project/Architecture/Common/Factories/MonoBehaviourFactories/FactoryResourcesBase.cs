using UnityEngine;
using Zenject;

public abstract class FactoryResourcesBase<TContract> : FactoryMonoBehaviourBase<TContract> where TContract : MonoBehaviour
{
    protected readonly IInstantiator _instantiator;

    public FactoryResourcesBase(IInstantiator instantiator)
    {
        _instantiator = instantiator;
    }

    protected override TSpecified CreateInternal<TSpecified>(Vector3 position, Quaternion rotation, Transform parent)
    {
        return _instantiator.InstantiatePrefabResourceForComponent<TSpecified>(GetPath(typeof(TSpecified).Name), position, rotation, parent);
    }

    protected virtual string GetPath(string prefabTitle)
    {
        return prefabTitle;
    }
}

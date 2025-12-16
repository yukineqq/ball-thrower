using UnityEngine;

public abstract class FactoryAddressablesBase<TContract> : FactoryMonoBehaviourBase<TContract> where TContract : MonoBehaviour
{
    protected readonly AddressablePrefabInstantiator _addressablePrefabInstantiator;

    public FactoryAddressablesBase(AddressablePrefabInstantiator addressablePrefabInstantiator)
    {
        _addressablePrefabInstantiator = addressablePrefabInstantiator;
    }

    protected override TSpecified CreateInternal<TSpecified>(Vector3 position, Quaternion rotation, Transform parent)
    {
        return _addressablePrefabInstantiator.InstantiatePreloadedAddressablePrefabForComponent<TSpecified>(typeof(TSpecified).Name, position, rotation, parent);
    }
}

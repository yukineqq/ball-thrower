using UnityEngine;

public sealed class ScreenSpaceUIFactory : FactoryAddressablesBase<WindowView>
{
    public ScreenSpaceUIFactory(AddressablePrefabInstantiator addressablePrefabInstantiator) : base(addressablePrefabInstantiator)
    {

    }

    protected override TSpecified CreateInternal<TSpecified>(Vector3 position, Quaternion rotation, Transform parent)
    {
        return _addressablePrefabInstantiator.InstantiatePreloadedAddressablePrefabForComponent<TSpecified>(typeof(TSpecified).Name.Replace("View", string.Empty), position, rotation, parent);
    }
}

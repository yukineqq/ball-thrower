using UnityEngine;

public sealed class EntitiesFactory : FactoryAddressablesBase<Entity>
{
    public EntitiesFactory(AddressablePrefabInstantiator addressablePrefabInstantiator) : base(addressablePrefabInstantiator)
    {

    }
}

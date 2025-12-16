using UnityEngine;

public sealed class EntitiesObjectPool : MonoBehaviourObjectPool<Entity, FactoryMonoBehaviourBase<Entity>>
{
    public override string RootFolder => "Entities";
    public EntitiesObjectPool(PoolParentContainersManager containersManager, FactoryMonoBehaviourBase<Entity> factory) : base(containersManager, factory)
    {

    }
}

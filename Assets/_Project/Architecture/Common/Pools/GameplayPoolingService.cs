using UnityEngine;
using Zenject;

public sealed class GameplayPoolingService : PoolingService
{
    public GameplayPoolingService(IInstantiator instantiator, IStaticDataService staticDataService) : base(instantiator, staticDataService)
    {

    }

    public override void Initialize()
    {
        RegisterPool<UIObjectPool>();
        RegisterPool<EntitiesObjectPool>();
    }
}

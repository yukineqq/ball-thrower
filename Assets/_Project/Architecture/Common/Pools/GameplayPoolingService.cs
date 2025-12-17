using UnityEngine;
using Zenject;

public sealed class GameplayPoolingService : PoolingService
{
    public GameplayPoolingService(IInstantiator instantiator, IStaticDataService staticDataService) : base(instantiator, staticDataService)
    {

    }

    public override void Initialize()
    {
        base.Initialize();

        RegisterPool<EntitiesObjectPool>();
    }
}

using UnityEngine;
using Zenject;

public sealed class MainMenuPoolingService : PoolingService
{
    public MainMenuPoolingService(IInstantiator instantiator, IStaticDataService staticDataService) : base(instantiator, staticDataService)
    {

    }
}

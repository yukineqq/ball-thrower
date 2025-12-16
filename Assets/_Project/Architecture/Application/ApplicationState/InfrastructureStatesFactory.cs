using Zenject;

public sealed class InfrastructureStatesFactory
{
    private readonly IInstantiator _instantiator;

    public InfrastructureStatesFactory(IInstantiator instantiator)
    {
        _instantiator = instantiator;
    }

    public TState Create<TState>() where TState : class, IExitableState
    {
        return _instantiator.Instantiate<TState>();
    }
}

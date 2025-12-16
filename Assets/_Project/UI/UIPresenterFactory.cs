using UnityEngine;
using Zenject;

public sealed class UIPresenterFactory
{
    private readonly IInstantiator _instantiator;
    
    public UIPresenterFactory(IInstantiator instantiator, UIViewInstantiator viewInstantiator)
    {
        _instantiator = instantiator;
    }

    public TPresenter CreateWindow<TPresenter>() where TPresenter : class, IWindowPresenter
    {
        return _instantiator.Instantiate<TPresenter>();
    }
}

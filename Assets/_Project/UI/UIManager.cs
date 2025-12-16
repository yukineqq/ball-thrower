using System;
using UnityEngine;
using Zenject;

public class UIManager : IInitializable, IDisposable
{
    protected readonly IInstantiator _instantiator;
    protected readonly SceneRootUI _sceneRootUi;
    protected readonly UIPresenterFactory _uiFactory;

    public UIManager(IInstantiator instantiator, SceneRootUI sceneRootUi, UIPresenterFactory uiFactory)
    {
        _instantiator = instantiator;
        _sceneRootUi = sceneRootUi;
        _uiFactory = uiFactory;
    }

    public void Initialize()
    {
        AddSubscriptions();
    }

    public void Dispose()
    {
        RemoveSubscriptions();
    }

    public TPopupPresenter CreatePopup<TPopupPresenter>() where TPopupPresenter : class, IWindowPresenter
    {
        var popup = CreateWindow<TPopupPresenter>();

        _sceneRootUi.OpenPopup(popup);

        return popup;
    }

    public TScreenPresenter CreateScreen<TScreenPresenter>() where TScreenPresenter : class, IWindowPresenter
    {
        var screen = CreateWindow<TScreenPresenter>();

        _sceneRootUi.AttachScreen(screen);

        return screen;
    }

    protected TPresenter CreateWindow<TPresenter>() where TPresenter : class, IWindowPresenter
    {
        var window = _uiFactory.CreateWindow<TPresenter>();

        window.Initialize();

        return window;
    }

    protected virtual void AddSubscriptions()
    {
        
    }

    protected virtual void RemoveSubscriptions()
    {
        
    }
}

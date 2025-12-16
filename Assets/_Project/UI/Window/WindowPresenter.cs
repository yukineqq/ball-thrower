using System;
using UnityEngine;

public abstract class WindowPresenter<TWindowView> : IWindowPresenter where TWindowView : WindowView
{
    protected readonly UIViewInstantiator _uiInstantiator;
    protected TWindowView _windowView;

    public Type ViewType => typeof(TWindowView);
    public abstract string WindowViewPrefabTitle { get; }

    public WindowPresenter(UIViewInstantiator uiInstantiator)
    {
        _uiInstantiator = uiInstantiator;

        _windowView = uiInstantiator.GetScreen<TWindowView>(WindowViewPrefabTitle);
    }

    public virtual void Initialize()
    {
        AddSubscriptions();
    }

    public void Close()
    {
        OnClose();

        Dispose();
    }

    public virtual void SetParent(Transform parent)
    {
        _windowView.transform.SetParent(parent, false);
        _windowView.transform.SetAsFirstSibling();
    }

    public virtual void Dispose()
    {
        RemoveSubscriptions();

        ReleaseView();
    }

    protected virtual void OnClose()
    {

    }

    protected virtual void AddSubscriptions()
    {
        _windowView.CloseRequested += Close;
    }

    protected virtual void RemoveSubscriptions()
    {
        _windowView.CloseRequested -= Close;
    }

    protected void ReleaseView()
    {
        if (_windowView != null && _windowView.gameObject != null)
        {
            _uiInstantiator.ReleaseWindowView(_windowView);
        }
    }
}

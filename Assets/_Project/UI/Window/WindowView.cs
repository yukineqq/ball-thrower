using System;
using UnityEngine;

public abstract class WindowView : MonoBehaviour, IPoolableObject
{
    public event Action CloseRequested;

    public void RequestClose()
    {
        CloseRequested?.Invoke();
        OnClose();
    }

    protected virtual void OnDestroy()
    {
        CloseRequested?.Invoke();
    }

    protected virtual void OnEnable()
    {
        AddSubscriptions();
    }

    protected virtual void OnDisable()
    {
        RemoveSubscriptions();
    }

    public virtual void OnAcquire()
    {

    }

    public virtual void OnRelease()
    {

    }

    protected virtual void OnClose()
    {

    }

    protected virtual void AddSubscriptions()
    {

    }

    protected virtual void RemoveSubscriptions()
    {

    }
}

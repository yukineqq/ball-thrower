using UnityEngine;

public abstract class SyncBaseState : ISyncBaseState
{
    public virtual void Enter()
    {
        AddSubscriptions();
    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {
        RemoveSubscriptions();
    }

    protected virtual void AddSubscriptions()
    {

    }

    protected virtual void RemoveSubscriptions()
    {

    }
}

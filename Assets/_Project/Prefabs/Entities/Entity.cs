using UnityEngine;

[SelectionBase]
public abstract class Entity : MonoBehaviour, IPoolableObject
{
    public virtual void OnAcquire()
    {

    }

    public virtual void OnRelease()
    {

    }
}

using UnityEngine;

public interface IPoolableObject
{
    public void OnAcquire();
    public void OnRelease();
}

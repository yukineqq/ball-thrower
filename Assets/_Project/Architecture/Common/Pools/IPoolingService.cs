using UnityEngine;

public interface IPoolingService
{
    public void Cleanup();
    public TObjectPool GetPool<TObjectPool>() where TObjectPool : class, IMonoBehaviourObjectPool;
    public bool RegisterPool<TObjectPool>() where TObjectPool : class, IMonoBehaviourObjectPool;
    public bool DestroyPool<TObjectPool>() where TObjectPool : class, IMonoBehaviourObjectPool;
}

using UnityEngine;

public interface IMonoBehaviourObjectPool
{
    public void ChangeCapacity(int capacity = 10);
    public TSpecified Get<TSpecified>() where TSpecified : MonoBehaviour;
    public TSpecified Get<TSpecified>(Vector3 position = default, Quaternion rotation = default, Transform parent = null, bool worldsPositionsStays = true) where TSpecified : MonoBehaviour;
    public void Release(MonoBehaviour objectToRelease);
    public void Clear(bool isPooled = true);
    public void Cleanup();
}

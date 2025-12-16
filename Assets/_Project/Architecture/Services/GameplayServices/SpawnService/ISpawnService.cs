using UnityEngine;

public interface ISpawnService
{
    public T Spawn<T>(string key, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : MonoBehaviour;
    public T SpawnFromPool<T>(Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : MonoBehaviour;
    public void ReleaseObject(MonoBehaviour instance);
    public void SetObjectTracking(MonoBehaviour instance, bool value);
}

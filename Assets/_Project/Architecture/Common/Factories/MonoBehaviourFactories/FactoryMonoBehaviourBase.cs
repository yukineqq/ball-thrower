using UnityEngine;

public abstract class FactoryMonoBehaviourBase<TContract> where TContract : MonoBehaviour
{
    public virtual TSpecified Create<TSpecified>() where TSpecified : MonoBehaviour
    {
        return CreateInternal<TSpecified>(default, default, null);
    }

    public virtual TSpecified Create<TSpecified>(Transform parent) where TSpecified : MonoBehaviour
    {
        return CreateInternal<TSpecified>(default, default, parent);
    }

    public virtual TSpecified Create<TSpecified>(Vector3 position = default, Quaternion rotation = default, Transform parent = null) where TSpecified : MonoBehaviour
    {
        return CreateInternal<TSpecified>(position, rotation, parent);
    }

    protected abstract TSpecified CreateInternal<TSpecified>(Vector3 position, Quaternion rotation, Transform parent) where TSpecified : MonoBehaviour;
}

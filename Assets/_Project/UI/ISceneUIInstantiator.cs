using UnityEngine;

public interface ISceneUIInstantiator
{
    public TView GetScreen<TView>(string prefabTitle) where TView : WindowView;
}

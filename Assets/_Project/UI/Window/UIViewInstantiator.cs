using UnityEngine;
using Zenject;

public sealed class UIViewInstantiator : ISceneUIInstantiator
{
    private readonly AddressablePrefabInstantiator _addressablePrefabInstantiator;

    public UIViewInstantiator(AddressablePrefabInstantiator addressablePrefabInstantiator)
    {
        _addressablePrefabInstantiator = addressablePrefabInstantiator;
    }

    public TWindowView GetScreen<TWindowView>(string prefabTitle) where TWindowView : WindowView
    {
        return _addressablePrefabInstantiator.InstantiatePreloadedAddressablePrefabForComponent<TWindowView>(prefabTitle);
    }

    public void ReleaseWindowView(WindowView view)
    {
        if (view != null && view.gameObject != null)
        {
            GameObject.Destroy(view.gameObject);
        }
    }
}

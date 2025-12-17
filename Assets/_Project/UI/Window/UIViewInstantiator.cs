using UnityEngine;
using Zenject;

public sealed class UIViewInstantiator : ISceneUIInstantiator
{
    private readonly AddressablePrefabInstantiator _addressablePrefabInstantiator;
    private readonly IPoolingService _poolingService;

    public UIViewInstantiator(AddressablePrefabInstantiator addressablePrefabInstantiator, IPoolingService poolingService)
    {
        _addressablePrefabInstantiator = addressablePrefabInstantiator;
        _poolingService = poolingService;
    }

    public TWindowView GetScreen<TWindowView>(string prefabTitle) where TWindowView : WindowView
    {
        TWindowView window = _poolingService.GetPool<UIObjectPool>().Get<TWindowView>(worldsPositionsStays: false);
        window.gameObject.SetActive(true);

        return window;
        //return _addressablePrefabInstantiator.InstantiatePreloadedAddressablePrefabForComponent<TWindowView>(prefabTitle);
    }

    public void ReleaseWindowView(WindowView view)
    {
        if (view != null && view.gameObject != null)
        {
            _poolingService.GetPool<UIObjectPool>().Release(view);
            return;
            //GameObject.Destroy(view.gameObject);
        }
    }
}

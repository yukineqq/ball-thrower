using UnityEngine;

public abstract class ScreenPresenter<TScreenView> : WindowPresenter<TScreenView> where TScreenView : ScreenView
{
    public ScreenPresenter(UIViewInstantiator uiInstantiator) : base(uiInstantiator)
    {

    }

    public override void SetParent(Transform parent)
    {
        base.SetParent(parent);

        _windowView.transform.SetAsLastSibling();
    }
}

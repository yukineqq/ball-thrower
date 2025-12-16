using UnityEngine;

public abstract class PopupPresenter<TPopupView> : WindowPresenter<TPopupView> where TPopupView : WindowView
{
    protected readonly IPopupCloser _popupCloser;

    public PopupPresenter(UIViewInstantiator uiInstantiator, IPopupCloser popupCloser) : base(uiInstantiator)
    {
        _popupCloser = popupCloser;
    }

    public override void SetParent(Transform parent)
    {
        base.SetParent(parent);
        _windowView.transform.SetAsLastSibling();
    }

    public override void Dispose()
    {
        base.Dispose();

        _popupCloser.RequestClosePopup(this);
    }
}

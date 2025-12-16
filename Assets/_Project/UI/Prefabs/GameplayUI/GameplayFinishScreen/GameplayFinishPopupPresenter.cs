using UnityEngine;

public sealed class GameplayFinishPopupPresenter : PopupPresenter<GameplayFinishPopupView>
{
    public override string WindowViewPrefabTitle => "GameplayFinishPopup";

    public GameplayFinishPopupPresenter(UIViewInstantiator uiViewInstantiator, IPopupCloser popupCloser) : base(uiViewInstantiator, popupCloser)
    {

    }
}

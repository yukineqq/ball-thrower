using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class GameplayPausePopupPresenter : PopupPresenter<GameplayPausePopupView>
{
    private readonly SceneStateMachine _sceneStateMachine;

    public override string WindowViewPrefabTitle => "GameplayPausePopup";

    public GameplayPausePopupPresenter(UIViewInstantiator uIViewInstantiator, IPopupCloser popupCloser, SceneStateMachine sceneStateMachine) : base(uIViewInstantiator, popupCloser)
    {
        _sceneStateMachine = sceneStateMachine;
    }

    protected override void AddSubscriptions()
    {
        base.AddSubscriptions();

        _windowView.ContinueButtonPressed += OnContinueButtonPressed;
        _windowView.QuitButtonPressed += OnQuitButtonPressed;
    }

    protected override void RemoveSubscriptions()
    {
        base.RemoveSubscriptions();

        _windowView.ContinueButtonPressed -= OnContinueButtonPressed;
        _windowView.QuitButtonPressed -= OnQuitButtonPressed;
    }

    private void OnContinueButtonPressed()
    {
        _sceneStateMachine.Enter<GameplayRegularState>().Forget();

        Close();
    }

    private void OnQuitButtonPressed()
    {
        _sceneStateMachine.Enter<GameplayQuitState>().Forget();
    }
}

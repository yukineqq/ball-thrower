using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class GameplayFinishPopupPresenter : PopupPresenter<GameplayFinishPopupView>
{
    private readonly SceneStateMachine _sceneStateMachine;
    private readonly PlayingSessionHelper _sessionHelper;

    public override string WindowViewPrefabTitle => "GameplayFinishPopup";

    public GameplayFinishPopupPresenter(UIViewInstantiator uiViewInstantiator, IPopupCloser popupCloser,
        SceneStateMachine sceneStateMachine, PlayingSessionHelper sessionHelper) : base(uiViewInstantiator, popupCloser)
    {
        _sceneStateMachine = sceneStateMachine;
        _sessionHelper = sessionHelper;
    }

    public override void Initialize()
    {
        base.Initialize();

        _windowView.FinishText = _sessionHelper.CurrentScore.ToString();
    }

    protected override void AddSubscriptions()
    {
        base.AddSubscriptions();

        _windowView.RestartButtonPressed += OnContinueButtonPressed;
        _windowView.QuitButtonPressed += OnQuitButtonPressed;
    }

    protected override void RemoveSubscriptions()
    {
        base.RemoveSubscriptions();

        _windowView.RestartButtonPressed -= OnContinueButtonPressed;
        _windowView.QuitButtonPressed -= OnQuitButtonPressed;
    }

    private void OnContinueButtonPressed()
    {
        _sceneStateMachine.Enter<GameplayPreparationState>().Forget();

        Close();
    }

    private void OnQuitButtonPressed()
    {
        _sceneStateMachine.Enter<GameplayQuitState>().Forget();
    }
}

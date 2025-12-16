using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class GameplayScreenPresenter : ScreenPresenter<GameplayScreenView>
{
    private readonly SceneStateMachine _sceneStateMachine;
    private readonly GameplayUIManager _uiManager;
    public override string WindowViewPrefabTitle => "GameplayScreen";

    public GameplayScreenPresenter(UIViewInstantiator uiInstantiator, SceneStateMachine sceneStateMachine, GameplayUIManager uiManager) : base(uiInstantiator)
    {
        _sceneStateMachine = sceneStateMachine;
        _uiManager = uiManager;
    }

    protected override void AddSubscriptions()
    {
        base.AddSubscriptions();

        _windowView.PauseButtonPressed += OnPauseButtonPressed;
    }

    protected override void RemoveSubscriptions()
    {
        base.RemoveSubscriptions();

        _windowView.PauseButtonPressed -= OnPauseButtonPressed;
    }

    private void OnPauseButtonPressed()
    {
        _sceneStateMachine.Enter<GameplayPauseState>().Forget();
        //_uiManager.CreatePopup<GameplayPausePopupPresenter>();
    }
}

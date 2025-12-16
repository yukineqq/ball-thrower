using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public sealed class MainMenuScreenPresenter : ScreenPresenter<MainMenuScreenView>
{
    private readonly GameStateMachine _gameStateMachine;

    public override string WindowViewPrefabTitle => "MainMenuScreen";

    public MainMenuScreenPresenter(UIViewInstantiator uiInstantiator, GameStateMachine gameStateMachine) : base(uiInstantiator)
    {
        _gameStateMachine = gameStateMachine;
    }

    protected override void AddSubscriptions()
    {
        base.AddSubscriptions();

        _windowView.PlayButtonClicked += OnPlayButtonClick;
    }

    protected override void RemoveSubscriptions()
    {
        base.RemoveSubscriptions();

        _windowView.PlayButtonClicked -= OnPlayButtonClick;
    }

    private void OnPlayButtonClick()
    {
        _gameStateMachine.Enter<GameGameplayState>().Forget();
    }
}

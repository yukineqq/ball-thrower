using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public sealed class MainMenuScreenPresenter : ScreenPresenter<MainMenuScreenView>
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly ISavingService _savingService;

    public override string WindowViewPrefabTitle => "MainMenuScreen";

    public MainMenuScreenPresenter(UIViewInstantiator uiInstantiator, GameStateMachine gameStateMachine, ISavingService savingService) : base(uiInstantiator)
    {
        _gameStateMachine = gameStateMachine;
        _savingService = savingService;
    }

    public override void Initialize()
    {
        base.Initialize();

        GameProgressStateProxy progressState = _savingService.GameProgressStateProxy;

        _windowView.Balance = progressState.Balance;
        _windowView.HighScore = progressState.HighScore;
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

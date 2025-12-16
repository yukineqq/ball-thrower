using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public sealed class GameplayStatusScreenPresenter : ScreenPresenter<GameplayStatusScreenView>
{
    private readonly PlayingSessionHelper _sessionHelper;
    private readonly GolfTimer _timer;

    public override string WindowViewPrefabTitle => "GameplayStatusScreen";

    public GameplayStatusScreenPresenter(UIViewInstantiator uiViewInstantiator, PlayingSessionHelper sessionHelper, GolfTimer timer) : base(uiViewInstantiator)
    {
        _sessionHelper = sessionHelper;
        _timer = timer;
    }

    public override void Initialize()
    {
        base.Initialize();

        _windowView.CoinsAmount = _sessionHelper.GameProgressState.Balance;
        _windowView.Score = 0;
    }

    protected override void AddSubscriptions()
    {
        base.AddSubscriptions();

        _sessionHelper.GameProgressState.BalanceChanged += OnBalanceChanged;
        _timer.SecondsLeftChanged += OnTimerValueChanged;
        _sessionHelper.CurrentScoreChanged += OnScoreChanged;
    }

    protected override void RemoveSubscriptions()
    {
        base.RemoveSubscriptions();

        _sessionHelper.GameProgressState.BalanceChanged -= OnBalanceChanged;
        _timer.SecondsLeftChanged -= OnTimerValueChanged;
        _sessionHelper.CurrentScoreChanged -= OnScoreChanged;
    }

    private void OnBalanceChanged(int value)
    {
        _windowView.CoinsAmount = value;
    }

    private void OnTimerValueChanged(float value)
    {
        _windowView.TimerStatus = value;
    }

    private void OnScoreChanged(int value)
    {
        _windowView.Score = value;
    }
}

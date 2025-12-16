using UnityEngine;

public sealed class GameplayStatusScreenPresenter : ScreenPresenter<GameplayStatusScreenView>
{
    private readonly GolfTimer _timer;

    public override string WindowViewPrefabTitle => "GameplayStatusScreen";

    public GameplayStatusScreenPresenter(UIViewInstantiator uiViewInstantiator, GolfTimer timer) : base(uiViewInstantiator)
    {
        _timer = timer;
    }

    protected override void AddSubscriptions()
    {
        base.AddSubscriptions();

        _timer.SecondsLeftChanged += OnTimerValueChanged;
    }

    protected override void RemoveSubscriptions()
    {
        base.RemoveSubscriptions();

        _timer.SecondsLeftChanged -= OnTimerValueChanged;
    }

    private void OnTimerValueChanged(float value)
    {
        _windowView.TimerStatus = value;
    }
}

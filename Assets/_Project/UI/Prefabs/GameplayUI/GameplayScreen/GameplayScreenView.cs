using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameplayScreenView : ScreenView
{
    [SerializeField] private Button _pauseButton;

    public event Action PauseButtonPressed;

    protected override void AddSubscriptions()
    {
        base.AddSubscriptions();

        _pauseButton.onClick.AddListener(OnPauseButtonPress);
    }

    protected override void RemoveSubscriptions()
    {
        base.RemoveSubscriptions();

        _pauseButton.onClick.RemoveListener(OnPauseButtonPress);
    }

    private void OnPauseButtonPress()
    {
        PauseButtonPressed?.Invoke();
    }
}

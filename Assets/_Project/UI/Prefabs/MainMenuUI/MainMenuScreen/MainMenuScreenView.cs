using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenuScreenView : ScreenView
{
    [SerializeField] private Button _playButton;

    public event Action PlayButtonClicked;

    protected override void AddSubscriptions()
    {
        base.AddSubscriptions();

        _playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    protected override void RemoveSubscriptions()
    {
        base.RemoveSubscriptions();

        _playButton.onClick.RemoveListener(OnPlayButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        PlayButtonClicked?.Invoke();
    }
}

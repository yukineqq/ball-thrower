using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameplayPausePopupView : PopupView
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _quitButton;

    public event Action ContinueButtonPressed;
    public event Action QuitButtonPressed;

    protected override void AddSubscriptions()
    {
        base.AddSubscriptions();

        _continueButton.onClick.AddListener(OnContinueButtonPress);
        _quitButton.onClick.AddListener(OnQuitButtonPress);
    }

    protected override void RemoveSubscriptions()
    {
        base.RemoveSubscriptions();

        _continueButton.onClick.RemoveListener(OnContinueButtonPress);
        _quitButton.onClick.RemoveListener(OnQuitButtonPress);
    }

    private void OnContinueButtonPress()
    {
        ContinueButtonPressed?.Invoke();
    }

    private void OnQuitButtonPress()
    {
        QuitButtonPressed?.Invoke();
    }
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameplayFinishPopupView : PopupView
{
    [SerializeField] private TextMeshProUGUI _finishScoreText;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _quitButton;

    public event Action RestartButtonPressed;
    public event Action QuitButtonPressed;

    public string FinishText { get => _finishScoreText.text; set => _finishScoreText.text = value; }

    protected override void AddSubscriptions()
    {
        base.AddSubscriptions();

        _restartButton.onClick.AddListener(OnRestartButtonPress);
        _quitButton.onClick.AddListener(OnQuitButtonPress);
    }

    protected override void RemoveSubscriptions()
    {
        base.RemoveSubscriptions();

        _restartButton.onClick.RemoveListener(OnRestartButtonPress);
        _quitButton.onClick.RemoveListener(OnQuitButtonPress);
    }

    private void OnRestartButtonPress()
    {
        RestartButtonPressed?.Invoke();
    }

    private void OnQuitButtonPress()
    {
        QuitButtonPressed?.Invoke();
    }
}

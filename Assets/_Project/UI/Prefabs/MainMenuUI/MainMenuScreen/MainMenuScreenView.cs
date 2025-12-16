using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenuScreenView : ScreenView
{
    [SerializeField] private Button _playButton;
    [SerializeField] private TextMeshProUGUI _balanceText;
    [SerializeField] private TextMeshProUGUI _highScoreText;

    public int Balance
    {
        get
        {
            if (int.TryParse(_balanceText.text, out int balance))
            {
                return balance;
            }

            return 0;
        }
        set => _balanceText.text = value.ToString();
    }

    public int HighScore
    {
        get
        {
            if (int.TryParse(_highScoreText.text, out int highScore))
            {
                return highScore;
            }

            return 0;
        }
        set => _highScoreText.text = value.ToString();
    }

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

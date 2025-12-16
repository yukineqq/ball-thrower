using System;
using TMPro;
using UnityEngine;

public sealed class GameplayStatusScreenView : ScreenView
{
    [SerializeField] private TextMeshProUGUI _coinsAmoutText;
    [SerializeField] private TextMeshProUGUI _timerStatusText;
    [SerializeField] private TextMeshProUGUI _scoreStatusText;

    public int CoinsAmount
    {
        get
        {
            if (int.TryParse(_coinsAmoutText.text, out int amount))
            {
                return amount;
            }

            return 0;
        }
        set => _coinsAmoutText.text = value.ToString();
    }

    public float TimerStatus
    {
        get 
        {
            if (float.TryParse(_timerStatusText.text, out float seconds))
            {
                return seconds;
            }

            return 0f;
        }
        set => _timerStatusText.text = value.ToString("F2");
    }

    public int Score
    {
        get 
        {
            if (int.TryParse(_scoreStatusText.text, out int score))
            {
                return score;
            }

            return 0;
        }
        set => _scoreStatusText.text = value.ToString();
    }
}

using System;
using TMPro;
using UnityEngine;

public sealed class GameplayStatusScreenView : ScreenView
{
    [SerializeField] private TextMeshProUGUI _coinsAmoutText;
    [SerializeField] private TextMeshProUGUI _timerStatusText;

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
            if (float.TryParse(_coinsAmoutText.text, out float seconds))
            {
                return seconds;
            }

            return 0f;
        }
        set => _timerStatusText.text = value.ToString("F2");
    }
}

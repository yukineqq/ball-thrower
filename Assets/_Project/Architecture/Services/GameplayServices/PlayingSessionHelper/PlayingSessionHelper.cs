using System;
using UnityEngine;

public sealed class PlayingSessionHelper
{
    private readonly ISavingService _savingService;

    public int CurrentScore { get; private set; } = 0;
    public GameProgressStateProxy GameProgressState => _savingService.GameProgressStateProxy;

    public event Action<int> CurrentScoreChanged;

    public PlayingSessionHelper(ISavingService savingService)
    {
        _savingService = savingService;
    }

    public void SaveProgress()
    {
        if (_savingService.GameProgressStateProxy.HighScore < CurrentScore)
        {
            _savingService.GameProgressStateProxy.SetHighScore(CurrentScore);
        }

        _savingService.SaveProgress();
    }

    public void AddBalance(int amountToAdd)
    {
        _savingService.GameProgressStateProxy.AddBalance(amountToAdd);
    }

    public bool SpendBalance(int amountToSpend)
    {
        return _savingService.GameProgressStateProxy.SubtractBalance(amountToSpend);
    }

    public void IncrementScore()
    {
        CurrentScore++;
        CurrentScoreChanged?.Invoke(CurrentScore);
    }

    public void DecrementScore()
    {
        CurrentScore = Mathf.Clamp(CurrentScore - 1, 0, int.MaxValue);
        CurrentScoreChanged?.Invoke(CurrentScore);
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        CurrentScoreChanged?.Invoke(CurrentScore);
    }
}

using System;
using UnityEngine;

public sealed class GameProgressStateProxy
{
    private readonly GameProgressState _origin;
    public int Balance => _origin.Balance;
    public int HighScore => _origin.HighScore;

    public event Action<int> BalanceChanged;
    public event Action<int> HighScoreChanged;

    public GameProgressStateProxy(GameProgressState origin)
    {
        _origin = origin;
    }

    public void AddBalance(int coinsToAdd)
    {
        coinsToAdd = Mathf.Abs(coinsToAdd);

        _origin.Balance += coinsToAdd;

        BalanceChanged?.Invoke(_origin.Balance);
    }

    public bool SubtractBalance(int coinsToSpend)
    {
        coinsToSpend = Mathf.Abs(coinsToSpend);

        if (coinsToSpend <= _origin.Balance)
        {
            _origin.Balance -= coinsToSpend;

            BalanceChanged?.Invoke(_origin.Balance);
            return true;
        }

        return false;
    }

    public void SetHighScore(int newHighscore)
    {
        _origin.HighScore = Mathf.Abs(newHighscore);

        HighScoreChanged?.Invoke(_origin.HighScore);
    }
}

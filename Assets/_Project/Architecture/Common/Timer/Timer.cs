using System;
using UnityEngine;
using Zenject;

public abstract class Timer : ITickable
{
    public bool Enabled { get; private set; } = false;
    public float SecondsLeft { get; private set; } = 0f;

    public event Action<float> SecondsLeftChanged;

    public void SetSecondsLeftAmount(float amount)
    {
        SecondsLeft = Math.Abs(amount);

        SecondsLeftChanged?.Invoke(SecondsLeft);
    }

    public void SetEnabled(bool value)
    {
        Enabled = value;
    }

    public void Tick()
    {
        if (Enabled)
        {
            SubtractSeconds(Time.deltaTime);
        }
    }

    public float AddSeconds(float amount)
    {
        SecondsLeft += Mathf.Abs(amount);

        SecondsLeftChanged?.Invoke(SecondsLeft);

        return SecondsLeft;
    }

    public float SubtractSeconds(float amount)
    {
        SecondsLeft -= Mathf.Abs(amount);

        if (SecondsLeft < 0)
        {
            SecondsLeft = 0f;
        }

        SecondsLeftChanged?.Invoke(SecondsLeft);

        return SecondsLeft;
    }
}

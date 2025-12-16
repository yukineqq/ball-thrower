using System;
using UnityEngine;

public interface IInputHandler
{
    public void Enable();
    public void Disable();
    public event Action<Vector2> PrimaryTapStarted;
    public event Action<Vector2> PrimaryTapPerformed;
    public event Action<Vector2> TouchPositionChanged;
}

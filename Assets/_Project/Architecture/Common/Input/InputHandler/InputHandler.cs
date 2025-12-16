using System;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class InputHandler : IInputHandler, IDisposable
{
    private readonly PlayerInputActions.GameplayActions _input;
    private Vector2 _currentPosition = Vector2.zero;

    public event Action<Vector2> PrimaryTapStarted;
    public event Action<Vector2> PrimaryTapPerformed;
    public event Action<Vector2> TouchPositionChanged;

    public InputHandler(PlayerInputActions inputActions)
    {
        _input = inputActions.Gameplay;
    }
    
    public void Dispose()
    {
        Disable();
    }

    public void Enable()
    {
        _input.Enable();

        _input.TouchPress.started += OnPrimaryTouchStarted;
        _input.TouchPress.canceled += OnPrimaryTouchPerformed;
        _input.TouchPosition.performed += OnPrimaryMove;
    }

    public void Disable()
    {
        _input.Disable();

        _input.TouchPress.started -= OnPrimaryTouchStarted;
        _input.TouchPress.canceled -= OnPrimaryTouchPerformed;
        _input.TouchPosition.performed -= OnPrimaryMove;
    }

    private void OnPrimaryTouchStarted(InputAction.CallbackContext context)
    {
        PrimaryTapStarted?.Invoke(_currentPosition);
    }

    private void OnPrimaryTouchPerformed(InputAction.CallbackContext context)
    {
        PrimaryTapPerformed?.Invoke(_currentPosition);
    }

    private void OnPrimaryMove(InputAction.CallbackContext context)
    {
        _currentPosition = context.ReadValue<Vector2>();
        TouchPositionChanged?.Invoke(_currentPosition);
    }
}

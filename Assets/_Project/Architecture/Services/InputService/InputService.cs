using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public sealed class InputService : IInputService, IInitializable, IDisposable
{
    private readonly PlayerInputActions _inputActions;
    private readonly IInputHandler _inputHandler;

    public InputService(PlayerInputActions inputActions, IInputHandler inputHandler)
    {
        _inputActions = inputActions;
        _inputHandler = inputHandler;
    }

    public void Initialize()
    {
        _inputActions.Enable();
    }

    public void Dispose()
    {
        _inputActions.Disable();

        _inputActions.Dispose();
    }

    public void ShowPointer(bool value = true)
    {
        if (value)
        {
            Cursor.visible = true;

#if UNITY_EDITOR
            Cursor.lockState = CursorLockMode.None;
            return;
#endif
            Cursor.lockState = CursorLockMode.Confined;
            return;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Enable()
    {
        _inputHandler.Enable();
    }

    public void Disable()
    {
        _inputHandler.Disable();
    }
}

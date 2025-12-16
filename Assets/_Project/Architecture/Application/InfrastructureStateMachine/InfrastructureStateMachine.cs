using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public abstract class InfrastructureStateMachine
{
    private readonly Dictionary<System.Type, IExitableState> _registratedStates;
    private IExitableState _currentState;

    public IExitableState CurrentState => _currentState;

    public InfrastructureStateMachine()
    {
        _registratedStates = new Dictionary<System.Type, IExitableState>();
    }

    public void RegisterState<TState>(TState state) where TState : class, IExitableState
    {
        if (_registratedStates.ContainsKey(typeof(TState)))
        {
            return;
        }

        _registratedStates.Add(typeof(TState), state);

        if (_registratedStates[typeof(TState)] == null)
        {
            Debug.Log("state is null");
        }
    }

    public async UniTask Enter<TState>() where TState : class, IExitableState
    {
        TState newState = await ChangeState<TState>();
        await newState.Enter();
    }

    public async UniTask<TState> ChangeState<TState>() where TState : class, IExitableState
    {
        if (_currentState != null)
        {
            await _currentState.Exit();
        }

        TState state = GetState<TState>();
        _currentState = state;

        return state;
    }

    private TState GetState<TState>() where TState : class, IExitableState
    {
        return _registratedStates[typeof(TState)] as TState;
    }
}

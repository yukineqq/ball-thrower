using System.Collections.Generic;

public abstract class SyncStateMachine<TStateContract> where TStateContract : ISyncBaseState
{
    private readonly Dictionary<System.Type, TStateContract> _registratedStates = new Dictionary<System.Type, TStateContract>();
    protected TStateContract _currentState;

    public void RegisterState<TConcreteState>(TStateContract state) where TConcreteState : class, TStateContract
    {
        if (_registratedStates.ContainsKey(typeof(TConcreteState)))
        {
            return;
        }

        _registratedStates.Add(typeof(TConcreteState), state);
    }

    public void EnterState<TConcreteState>() where TConcreteState : class, TStateContract
    {
        TConcreteState newState = ChangeState<TConcreteState>();
        newState.Enter();
    }

    public void UpdateState()
    {
        if (_currentState != null)
        {
            _currentState.Update();
        }
    }

    public TConcreteState ChangeState<TConcreteState>() where TConcreteState : class, TStateContract
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }

        TConcreteState state = GetState<TConcreteState>();
        _currentState = state;

        return state;
    }

    private TConcreteState GetState<TConcreteState>() where TConcreteState : class, TStateContract
    {
        return _registratedStates[typeof(TConcreteState)] as TConcreteState;
    }
}

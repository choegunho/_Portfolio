using UnityEngine;

public class StateMachine
{
    IState _currentState;

    public void ChangeState(IState newState)
    {
        if(_currentState != null)
        {
            _currentState.Exit();
        }

        _currentState = newState;

        if(_currentState != null)
        {
            _currentState.Enter();
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if(_currentState != null)
        {
            _currentState.Execute();
        }
    }

    public IState GetCurrentState()
    {
        return _currentState;
    }
}

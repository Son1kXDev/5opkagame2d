using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private IState _currentState;
    private IState _previousState;

    public void ChangeState(IState newState)
    {
        _currentState?.Exit();
        _previousState = _currentState;
        _currentState = newState;
    }

    public void ExecuteStateUpdate()
    {
        _currentState?.Execute();
    }

    public void BackToPreviousState()
    {
        ChangeState(_previousState);
    }

}

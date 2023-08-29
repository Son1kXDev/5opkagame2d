using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStateMachine
{
    public EnemyState CurrentEnemyState { get; private set; }

    public void Initialize(EnemyState initialState)
    {
        CurrentEnemyState = initialState;
        CurrentEnemyState.EnterState();
    }

    public void ChangeState(EnemyState newState)
    {
        CurrentEnemyState.ExitState();
        CurrentEnemyState = newState;
        CurrentEnemyState.EnterState();
    }

}

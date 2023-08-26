using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum EnemyState { Patrol, Follow, Attack, PreparingForDeath, Dead }

public class EnemyManager : Health
{

    public Action<EnemyState> OnEnemyStateUpdated;
    public EnemyState CurrentState => _currentState;
    [SerializeField, ShowOnly] private EnemyState _currentState;

    private void Awake() => InitializeEnemy();

    public void InitializeEnemy()
    {
        InitializeHealth();
        UpdateEnemyState(EnemyState.Patrol);
    }
    public void UpdateEnemyState(EnemyState newState)
    {
        if (newState == _currentState) return;

        _currentState = newState;

        // Debug.Log($"Current state: {_currentState}");
        // OnEnemyStateUpdated?.Invoke(newState);
    }

    protected override void VisualizeHealth()
    {
        //
    }

    protected override void OnDeath()
    {
        //
    }
}

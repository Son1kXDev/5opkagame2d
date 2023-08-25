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

    [SerializeField] LayerMask _groundLayer;

    private Rigidbody2D _rigidbody;

    private void Start() => InitializeEnemy();

    public void InitializeEnemy()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        InitializeHealth();
        UpdateEnemyState(EnemyState.Patrol);
    }
    public void UpdateEnemyState(EnemyState newState)
    {
        if (newState == _currentState) return;
        if (!IsRightState(newState)) return;

        _currentState = newState;

        // Debug.Log($"Current state: {_currentState}");
        // OnEnemyStateUpdated?.Invoke(newState);
    }

    private bool IsRightState(EnemyState newState)
    {
        if (_currentState == EnemyState.Dead) return false;

        else return true;
    }

    public void Jump()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, _groundLayer);
        if (hit.collider != null)
            _rigidbody.AddForce(new Vector2(0f, 250f));
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

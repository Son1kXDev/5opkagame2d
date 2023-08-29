using System;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class Enemy : Health
{

    [field: Header("Config")]
    [field: SerializeField, StatusIcon] public EnemyConfig EnemyConfig { get; private set; }

    [field: Header("Patrol System")]
    [field: SerializeField] public List<Transform> PatrolPoints { get; private set; } = new List<Transform>();

    public EnemyStateMachine StateMachine { get; private set; }
    public EnemyPatrolState PatrolState { get; private set; }
    public EnemyFollowState FollowState { get; private set; }
    public EnemySearchState SearchState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    public Transform Player { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public Collider2D Collider { get; private set; }
    public Path Path { get; set; }
    public Seeker Seeker { get; private set; }

    private void Awake() => InitializeEnemy();

    public void InitializeEnemy()
    {
        if (EnemyConfig == null)
            throw new NullReferenceException("Enemy Config is null");

        InitializeHealth(EnemyConfig.MaximumHealth);

        Seeker = GetComponent<Seeker>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        StateMachine = new EnemyStateMachine();
        PatrolState = new EnemyPatrolState(this, StateMachine);
        FollowState = new EnemyFollowState(this, StateMachine);
        SearchState = new EnemySearchState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);

        StateMachine.Initialize(PatrolState);

        InvokeRepeating(nameof(UpdateEnemy), 0f, EnemyConfig.PathUpdateSeconds);
    }

    public void UpdateEnemy() => StateMachine.CurrentEnemyState.UpdateState();

    protected override void FixedRun() => StateMachine.CurrentEnemyState.FixedUpdateState();

    protected override void VisualizeHealth()
    {
        Debug.SetColor(Color.blue);
        Debug.Log("Taking damage! Current Health: " + _currentHealth);
    }

    protected override void OnDeath()
    {
        Debug.Log("Enemy died!");
    }

    private void OnDrawGizmos()
    {
        if (EnemyConfig == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, EnemyConfig.ActivationDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EnemyConfig.AttackDistance);
    }
}

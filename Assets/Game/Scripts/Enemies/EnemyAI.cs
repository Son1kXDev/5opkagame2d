using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

[Component("Enemy AI")]
public class EnemyAI : MonoBehaviour
{
    [Header("Config")]
    [SerializeField, StatusIcon] private Enemy _enemyConfig;

    [Header("Patrol System")]
    [SerializeField] private List<Transform> _patrolPoints = new List<Transform>();

    private int _currentWaypointIndex = 0;
    private int _currentPatrolPointIndex = 0;
    private float _cooldownTimer = 0f;
    private Transform _player;
    private Transform _currentTarget;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private Path _path;
    private Seeker _seeker;
    private EnemyManager _enemyManager;

    private void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _enemyManager = GetComponent<EnemyManager>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        if (_enemyConfig == null)
            throw new NullReferenceException("Enemy Config is null");

        InvokeRepeating(nameof(UpdatePath), 0f, _enemyConfig.PathUpdateSeconds);
    }

    private void UpdatePath()
    {
        switch (_enemyManager.CurrentState)
        {
            case EnemyState.Patrol:
                if (PlayerInViewDistance()) _enemyManager.UpdateEnemyState(EnemyState.Follow);

                if (_currentTarget == null) _currentTarget = _patrolPoints[_currentPatrolPointIndex];

                float distance = Vector2.Distance(_rigidbody.position, _currentTarget.position);
                if (distance <= 1f)
                {
                    _currentPatrolPointIndex++;
                    if (_currentPatrolPointIndex >= _patrolPoints.Count) _currentPatrolPointIndex = 0;
                    _currentTarget = _patrolPoints[_currentPatrolPointIndex];
                }

                if (_seeker.IsDone()) _seeker.StartPath(_rigidbody.position, _currentTarget.position, OnPathComplete);
                break;

            case EnemyState.Follow:
                if (PlayerInViewDistance() == false) _enemyManager.UpdateEnemyState(EnemyState.Patrol);
                if (PlayerInAttackDistance()) _enemyManager.UpdateEnemyState(EnemyState.Attack);

                if (_seeker.IsDone()) _seeker.StartPath(_rigidbody.position, _player.position, OnPathComplete);
                break;

            case EnemyState.Attack:
                if (PlayerInAttackDistance() == false) _enemyManager.UpdateEnemyState(EnemyState.Follow);

                _cooldownTimer += _enemyConfig.PathUpdateSeconds;
                if (_cooldownTimer >= _enemyConfig.AttackCooldown)
                {
                    _cooldownTimer = 0;
                    PlayerController.Instance.TakeDamage(_enemyConfig.Damage);
                }
                break;

            default:
                break;
        }

    }


    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _currentWaypointIndex = 0;
        }
    }

    private void FixedUpdate()
    {
        PathFollow();
    }

    private void PathFollow()
    {
        if (_path == null) return;
        if (_currentWaypointIndex >= _path.vectorPath.Count) return;

        RaycastHit2D isGrounded = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0, Vector2.down, 0.1f, _enemyConfig.GroundLayer);

        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypointIndex] - _rigidbody.position).normalized;
        Vector2 force = direction * _enemyConfig.Speed * Time.fixedDeltaTime;

        if (_enemyConfig.DirectionLookEnabled)
        {
            if (_rigidbody.velocity.x > 0.05f)
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else if (_rigidbody.velocity.x < -0.05f)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }


        if (isGrounded && _enemyConfig.JumpEnabled)
        {
            if (direction.y > _enemyConfig.JumpNodeHeightRequirement)
                _rigidbody.AddForce(Vector2.up * _enemyConfig.Speed * _enemyConfig.JumpModifier);
        }

        force.y = 0;
        _rigidbody.AddForce(force);

        float distance = Vector2.Distance(_rigidbody.position, _path.vectorPath[_currentWaypointIndex]);
        if (distance < _enemyConfig.NextWaypointDistance)
            _currentWaypointIndex++;
    }

    private bool PlayerInViewDistance()
    {
        return Vector2.Distance(transform.position, _player.position) < _enemyConfig.ActivationDistance;
    }

    private bool PlayerInAttackDistance()
    {
        return Vector2.Distance(transform.position, _player.position) < _enemyConfig.AttackDistance;
    }

    private void OnDrawGizmos()
    {
        if (_enemyConfig == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _enemyConfig.ActivationDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _enemyConfig.AttackDistance);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

[Component("Enemy AI")]
public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding System")]
    [SerializeField, StatusIcon(minValue: 0f)] private float _activationDistance = 8.5f;
    [SerializeField, StatusIcon(minValue: 0f)] private float _pathUpdateSeconds = .1f;

    [Header("Patrol System")]
    [SerializeField] private List<Transform> _patrolPoints = new List<Transform>();

    [Header("Attack System")]
    [SerializeField, StatusIcon(minValue: 0f)] private float _attackDistance = 1.5f;
    [SerializeField, StatusIcon(minValue: 0f)] private float _attackCooldown = 2.5f;
    [SerializeField, StatusIcon(minValue: 0)] private int _damage = 10;

    [Header("Physics")]
    [SerializeField, StatusIcon(minValue: 0f)] private float _speed = 200f;
    [SerializeField, StatusIcon(minValue: 0f)] private float _nextWaypointDistance = 3f;
    [SerializeField, StatusIcon(minValue: 0f)] private float _jumpNodeHeightRequirement = 0.8f;
    [SerializeField, StatusIcon(minValue: 0f)] private float _jumpModifier = 0.3f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Custom Behaviour")]
    [SerializeField] private bool _jumpEnabled = true;
    [SerializeField] private bool _directionLookEnabled = true;

    private Path _path;
    private int _currentWaypointIndex = 0;
    private int _currentPatrolPointIndex = 0;
    private float _cooldownTimer = 0f;
    private Seeker _seeker;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private EnemyManager _enemyManager;
    private Transform _player;
    private Transform _currentTarget;

    private void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _enemyManager = GetComponent<EnemyManager>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating(nameof(UpdatePath), 0f, _pathUpdateSeconds);
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

                _cooldownTimer += _pathUpdateSeconds;
                if (_cooldownTimer >= _attackCooldown)
                {
                    _cooldownTimer = 0;
                    PlayerController.Instance.TakeDamage(_damage);
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

        RaycastHit2D isGrounded = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0, Vector2.down, 0.1f, _groundLayer);

        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypointIndex] - _rigidbody.position).normalized;
        Vector2 force = direction * _speed * Time.fixedDeltaTime;

        if (_directionLookEnabled)
        {
            if (_rigidbody.velocity.x > 0.05f)
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else if (_rigidbody.velocity.x < -0.05f)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }


        if (isGrounded && _jumpEnabled)
        {
            if (direction.y > _jumpNodeHeightRequirement)
                _rigidbody.AddForce(Vector2.up * _speed * _jumpModifier);
        }

        force.y = 0;
        _rigidbody.AddForce(force);

        float distance = Vector2.Distance(_rigidbody.position, _path.vectorPath[_currentWaypointIndex]);
        if (distance < _nextWaypointDistance)
            _currentWaypointIndex++;
    }

    private bool PlayerInViewDistance()
    {
        return Vector2.Distance(transform.position, _player.position) < _activationDistance;
    }

    private bool PlayerInAttackDistance()
    {
        return Vector2.Distance(transform.position, _player.position) < _attackDistance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _activationDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackDistance);
    }
}

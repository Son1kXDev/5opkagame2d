using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

[Component("Enemy AI")]
public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    [SerializeField, StatusIcon] private Transform _target;
    [SerializeField, StatusIcon(minValue: 0f)] private float _activationDistance = 50f;
    [SerializeField, StatusIcon(minValue: 0f)] private float _pathUpdateSeconds = .5f;

    [Header("Physics")]
    [SerializeField, StatusIcon(minValue: 0f)] private float _speed = 200f;
    [SerializeField, StatusIcon(minValue: 0f)] private float _nextWaypointDistance = 3f;
    [SerializeField, StatusIcon(minValue: 0f)] private float _jumpNodeHeightRequirement = 0.8f;
    [SerializeField, StatusIcon(minValue: 0f)] private float _jumpModifier = 0.3f;
    [SerializeField, StatusIcon(minValue: 0f)] private float _jumpCheckOffset = 0.1f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Custom Behaviour")]
    [SerializeField] private bool _followEnabled = true;
    [SerializeField] private bool _jumpEnabled = true;
    [SerializeField] private bool _directionLookEnabled = true;

    private Path _path;
    private int _currentWaypointIndex = 0;
    private Seeker _seeker;
    private Rigidbody2D _rigidbody;

    private Collider2D _collider;

    private void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        InvokeRepeating(nameof(UpdatePath), 0f, _pathUpdateSeconds);
    }

    private void UpdatePath()
    {
        if (_followEnabled && TargetInDistance() && _seeker.IsDone())
            _seeker.StartPath(_rigidbody.position, _target.position, OnPathComplete);
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
        if (TargetInDistance() && _followEnabled)
            PathFollow();
    }

    private void PathFollow()
    {
        if (_path == null) return;
        if (_currentWaypointIndex >= _path.vectorPath.Count) return;

        //_isGrounded = Physics2D.Raycast(transform.position, -Vector3.up, GetComponent<Collider2D>().bounds.extents.y + _jumpCheckOffset);

        RaycastHit2D isGrounded = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0, Vector2.down, 0.1f, _groundLayer);

        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypointIndex] - _rigidbody.position).normalized;
        Vector2 force = direction * _speed * Time.fixedDeltaTime;


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

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, _target.position) < _activationDistance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, _activationDistance);
    }
}

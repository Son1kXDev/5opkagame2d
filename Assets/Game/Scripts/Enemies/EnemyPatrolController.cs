using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolController : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField, StatusIcon] private Transform _leftEdge;
    [SerializeField, StatusIcon] private Transform _rightEdge;

    [Header("Enemy")]
    [SerializeField, StatusIcon] private Transform _enemy;
    [SerializeField, StatusIcon] private EnemyManager _enemyManager;
    [SerializeField, StatusIcon] private EnemyControllerLegacy _enemyController;

    [Header("Movement")]
    [SerializeField, StatusIcon(minValue: 0f)] private float _speed;
    [SerializeField] private LayerMask _groundLayer;

    private Vector3 _initialScale;
    private bool _isMovingLeft;

    private void Awake()
    {
        _initialScale = _enemy.localScale;
    }

    private void Update()
    {
        switch (_enemyManager.CurrentState)
        {
            case EnemyState.Patrol:
                IdlePatrol();
                break;
            case EnemyState.Follow:
                FollowPlayer();
                break;
            default:
                _enemy.localScale = _enemy.localScale;
                _enemy.position = _enemy.position;
                break;
        }
    }

    private void IdlePatrol()
    {
        if (_isMovingLeft)
        {
            if (_enemy.position.x >= _leftEdge.position.x) MoveInDirection(-1);
            else _isMovingLeft = !_isMovingLeft;
        }
        else
        {
            if (_enemy.position.x <= _rightEdge.position.x) MoveInDirection(1);
            else _isMovingLeft = !_isMovingLeft;
        }
    }

    private void FollowPlayer()
    {
        Vector3 direction = (PlayerController.Instance.transform.position - _enemy.position).normalized;
        int dir = Mathf.RoundToInt(direction.x);
        MoveInDirection(dir);
    }

    private void MoveInDirection(int _direction)
    {
        _enemy.localScale = new Vector3(Mathf.Abs(_initialScale.x) * _direction, _initialScale.y, _initialScale.z);

        Vector3 offset = new Vector3(_direction * _enemy.position.x, _enemy.position.y, _enemy.position.z);
        _enemyController.Move(transform.position + (offset * _speed * Time.deltaTime));
    }

    private void FixedUpdate()
    {
        switch (_enemyManager.CurrentState)
        {
            case EnemyState.Patrol:
            case EnemyState.Follow:
                CheckForObstacle();
                break;
            default:
                break;
        }
    }

    private void CheckForObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(_enemy.position, Vector2.right * _enemy.localScale, 1.5f, _groundLayer);
        if (hit.collider != null)
            _enemyManager.Jump();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_enemy.position, (Vector2)_enemy.position + Vector2.right * _enemy.localScale * 1.5f);
    }
}

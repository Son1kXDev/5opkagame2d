using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(EnemyController))]
public class EnemyMeleeController : MonoBehaviour
{
    //todo: перенести их в scriptable object
    [Header("View")]
    [SerializeField] private float _viewVerticalRange;
    [SerializeField] private float _viewHorizontalRange;
    [SerializeField] private float _viewColliderDistance;

    [Header("Attack")]
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _attackHorizontalRange;
    [SerializeField] private float _attackVerticalRange;
    [SerializeField] private float _attackColliderDistance;
    [SerializeField] private int _damage;

    [Header("Other")]
    [SerializeField, StatusIcon] private BoxCollider2D _rangeCollider;
    [SerializeField] private LayerMask _playerMask;

    private float _cooldownTimer = Mathf.Infinity;
    private EnemyController _enemyController;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        switch (_enemyController.CurrentState)
        {
            case EnemyState.Patrol:
                if (PlayerInView()) _enemyController.UpdateEnemyState(EnemyState.Follow);
                break;

            case EnemyState.Follow:
                if (PlayerInSight()) _enemyController.UpdateEnemyState(EnemyState.Attack);
                if (PlayerInView() == false) _enemyController.UpdateEnemyState(EnemyState.Patrol);
                break;

            case EnemyState.Attack:
                _cooldownTimer += Time.deltaTime;
                if (_cooldownTimer >= _attackCooldown)
                {
                    _cooldownTimer = 0;
                    //todo: set trigger animation
                    DamagePlayer();
                }
                if (PlayerInSight() == false) _enemyController.UpdateEnemyState(EnemyState.Follow);
                break;
        }

    }

    private bool PlayerInView()
    {
        RaycastHit2D hit =
        Physics2D.BoxCast(_rangeCollider.bounds.center + _viewColliderDistance * _viewHorizontalRange * transform.localScale.x * transform.right,
        new Vector3(_rangeCollider.bounds.size.x * _viewHorizontalRange, _rangeCollider.bounds.size.y * _viewVerticalRange,
        _rangeCollider.bounds.size.z),
        0, Vector2.left, 0, _playerMask);
        return hit.collider != null;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit =
        Physics2D.BoxCast(_rangeCollider.bounds.center + _attackColliderDistance * _attackHorizontalRange * transform.localScale.x
        * transform.right,
        new Vector3(_rangeCollider.bounds.size.x * _attackHorizontalRange, _rangeCollider.bounds.size.y * _attackVerticalRange,
        _rangeCollider.bounds.size.z),
        0, Vector2.left, 0, _playerMask);
        return hit.collider != null;
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
            PlayerController.Instance.TakeDamage(_damage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_rangeCollider.bounds.center + _attackColliderDistance * _attackHorizontalRange * transform.localScale.x
        * transform.right,
        new Vector3(_rangeCollider.bounds.size.x * _attackHorizontalRange, _rangeCollider.bounds.size.y * _attackVerticalRange,
        _rangeCollider.bounds.size.z));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_rangeCollider.bounds.center + _viewColliderDistance * _viewHorizontalRange * transform.localScale.x * transform.right,
        new Vector3(_rangeCollider.bounds.size.x * _viewHorizontalRange, _rangeCollider.bounds.size.y * _viewVerticalRange,
        _rangeCollider.bounds.size.z));
    }
}

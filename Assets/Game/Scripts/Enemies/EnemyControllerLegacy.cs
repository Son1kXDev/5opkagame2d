using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(EnemyManager))]
public class EnemyControllerLegacy : MonoBehaviour
{
    [SerializeField, StatusIcon] private Enemy _enemyConfig;

    [Header("Other")]
    [SerializeField, StatusIcon] private BoxCollider2D _rangeCollider;
    [SerializeField] private LayerMask _playerMask;

    private float _cooldownTimer = Mathf.Infinity;
    private EnemyManager _enemyManager;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _enemyManager = GetComponent<EnemyManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        switch (_enemyManager.CurrentState)
        {
            case EnemyState.Patrol:
                if (PlayerInView()) _enemyManager.UpdateEnemyState(EnemyState.Follow);
                break;

            case EnemyState.Follow:
                if (PlayerInSight()) _enemyManager.UpdateEnemyState(EnemyState.Attack);
                if (PlayerInView() == false) _enemyManager.UpdateEnemyState(EnemyState.Patrol);
                break;

            case EnemyState.Attack:
                _cooldownTimer += Time.deltaTime;
                if (_cooldownTimer >= _enemyConfig.AttackDelay)
                {
                    _cooldownTimer = 0;
                    //todo: set trigger animation
                    DamagePlayer();
                }
                if (PlayerInSight() == false) _enemyManager.UpdateEnemyState(EnemyState.Follow);
                break;
        }

    }

    public void Move(Vector2 direction)
    {
        _rigidbody.MovePosition(direction);
    }

    private bool PlayerInView()
    {
        RaycastHit2D hit =
        Physics2D.BoxCast(_rangeCollider.bounds.center + _enemyConfig.FieldColliderDistance * _enemyConfig.FieldHorizontalRange
        * transform.localScale.x * transform.right, new Vector3(_rangeCollider.bounds.size.x * _enemyConfig.FieldHorizontalRange,
        _rangeCollider.bounds.size.y * _enemyConfig.FieldVerticalRange, _rangeCollider.bounds.size.z),
        0, Vector2.left, 0, _playerMask);
        return hit.collider != null;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit =
        Physics2D.BoxCast(_rangeCollider.bounds.center + _enemyConfig.AttackColliderDistance * _enemyConfig.AttackHorizontalRange
        * transform.localScale.x * transform.right, new Vector3(_rangeCollider.bounds.size.x * _enemyConfig.AttackHorizontalRange,
        _rangeCollider.bounds.size.y * _enemyConfig.AttackVerticalRange, _rangeCollider.bounds.size.z),
        0, Vector2.left, 0, _playerMask);
        return hit.collider != null;
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
            PlayerController.Instance.TakeDamage(_enemyConfig.Damage);
    }

    private void OnDrawGizmos()
    {
        if (_enemyConfig == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_rangeCollider.bounds.center + _enemyConfig.AttackColliderDistance * _enemyConfig.AttackHorizontalRange
        * transform.localScale.x * transform.right, new Vector3(_rangeCollider.bounds.size.x * _enemyConfig.AttackHorizontalRange,
        _rangeCollider.bounds.size.y * _enemyConfig.AttackVerticalRange, _rangeCollider.bounds.size.z));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_rangeCollider.bounds.center + _enemyConfig.FieldColliderDistance * _enemyConfig.FieldHorizontalRange
        * transform.localScale.x * transform.right, new Vector3(_rangeCollider.bounds.size.x * _enemyConfig.FieldHorizontalRange,
        _rangeCollider.bounds.size.y * _enemyConfig.FieldVerticalRange, _rangeCollider.bounds.size.z));
    }
}

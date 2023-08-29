using Pathfinding;
using UnityEngine;

public class EnemyState
{
    protected Enemy _enemy;
    protected EnemyStateMachine _enemyStateMachine;

    protected int _currentPatrolPointIndex;
    protected int _currentWaypointIndex = 0;

    public EnemyState(Enemy enemy, EnemyStateMachine enemyStateMachine)
    {
        _enemy = enemy;
        _enemyStateMachine = enemyStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState()
    {
        if (_enemy.Path == null) return;
        if (_currentWaypointIndex >= _enemy.Path.vectorPath.Count) return;

        RaycastHit2D isGrounded = Physics2D.BoxCast(_enemy.Collider.bounds.center, _enemy.Collider.bounds.size, 0, Vector2.down, 0.1f,
         _enemy.EnemyConfig.GroundLayer);

        Vector2 direction = ((Vector2)_enemy.Path.vectorPath[_currentWaypointIndex] - _enemy.Rigidbody.position).normalized;
        Vector2 force = direction * _enemy.EnemyConfig.Speed * Time.fixedDeltaTime;

        if (_enemy.EnemyConfig.DirectionLookEnabled)
        {
            if (_enemy.Rigidbody.velocity.x > 0.05f)
                _enemy.transform.localScale =
                new Vector3(-1f * Mathf.Abs(_enemy.transform.localScale.x), _enemy.transform.localScale.y, _enemy.transform.localScale.z);
            else if (_enemy.Rigidbody.velocity.x < -0.05f)
                _enemy.transform.localScale =
                new Vector3(Mathf.Abs(_enemy.transform.localScale.x), _enemy.transform.localScale.y, _enemy.transform.localScale.z);
        }


        if (isGrounded && _enemy.EnemyConfig.JumpEnabled)
        {
            if (direction.y > _enemy.EnemyConfig.JumpNodeHeightRequirement)
                _enemy.Rigidbody.AddForce(Vector2.up * _enemy.EnemyConfig.Speed * _enemy.EnemyConfig.JumpForce);
        }

        force.y = 0;
        _enemy.Rigidbody.AddForce(force);

        float distance = Vector2.Distance(_enemy.Rigidbody.position, _enemy.Path.vectorPath[_currentWaypointIndex]);
        if (distance < _enemy.EnemyConfig.NextWaypointDistance)
            _currentWaypointIndex++;
    }
    public virtual void AnimationTriggerEvent() { }

    protected void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _enemy.Path = p;
            _currentWaypointIndex = 0;
        }
    }

    protected bool PlayerInViewDistance()
    {
        return Vector2.Distance(_enemy.transform.position, _enemy.Player.position) < _enemy.EnemyConfig.ActivationDistance;
    }

    protected bool PlayerInAttackDistance()
    {
        return Vector2.Distance(_enemy.transform.position, _enemy.Player.position) < _enemy.EnemyConfig.AttackDistance;
    }
}

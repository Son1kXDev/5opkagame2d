using UnityEngine;

public class EnemySearchState : EnemyState
{
    private float _memoryTimer = 0;
    private Vector2 _lastKnownPosition;
    public EnemySearchState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void EnterState()
    {
        Debug.Log("Entering Search State");
        _lastKnownPosition = _enemy.Player.position;
        _memoryTimer = 0;
    }
    public override void ExitState()
    {
        Debug.Log("Exiting Search State");
    }
    public override void UpdateState()
    {
        if (PlayerInViewDistance()) _enemyStateMachine.ChangeState(_enemy.FollowState);

        if (PlayerInAttackDistance()) _enemyStateMachine.ChangeState(_enemy.AttackState);

        if (_enemy.Seeker.IsDone()) _enemy.Seeker.StartPath(_enemy.Rigidbody.position, _lastKnownPosition, OnPathComplete);

        if (Vector2.Distance(_enemy.Rigidbody.position, _lastKnownPosition) < 0.5f)
        {
            _memoryTimer += _enemy.EnemyConfig.PathUpdateSeconds;
            if (_memoryTimer >= _enemy.EnemyConfig.MemoryDelay)
                _enemyStateMachine.ChangeState(_enemy.PatrolState);
        }
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }
    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private float _cooldownTimer = 0;

    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void EnterState()
    {
        Debug.Log("Entering Attack State");
        _cooldownTimer = 0;
    }
    public override void ExitState()
    {
        Debug.Log("Exiting Attack State");
    }
    public override void UpdateState()
    {
        if (PlayerInAttackDistance() == false) _enemyStateMachine.ChangeState(_enemy.FollowState);

        _cooldownTimer += _enemy.EnemyConfig.PathUpdateSeconds;
        if (_cooldownTimer >= _enemy.EnemyConfig.AttackCooldown)
        {
            _cooldownTimer = 0;
            Player.Instance.TakeDamage(_enemy.EnemyConfig.Damage);
        }
    }
    public override void FixedUpdateState() { }
    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }

}

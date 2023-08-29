public class EnemyFollowState : EnemyState
{
    public EnemyFollowState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void EnterState()
    {
        Debug.Log("Entering Follow State");
    }
    public override void ExitState()
    {
        Debug.Log("Exiting Follow State");
    }
    public override void UpdateState()
    {
        if (PlayerInViewDistance() == false) _enemyStateMachine.ChangeState(_enemy.SearchState);

        if (PlayerInAttackDistance()) _enemyStateMachine.ChangeState(_enemy.AttackState);

        if (_enemy.Seeker.IsDone()) _enemy.Seeker.StartPath(_enemy.Rigidbody.position, _enemy.Player.position, OnPathComplete);
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



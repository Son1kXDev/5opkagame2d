using UnityEngine;
namespace Enjine
{
    public class EnemyPatrolState : EnemyState
    {
        private Transform _currentTarget;

        public EnemyPatrolState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

        public override void EnterState()
        {
            Debug.Log("Entering Patrol State");
            _currentTarget = _enemy.PatrolPoints[_currentPatrolPointIndex];
        }
        public override void ExitState()
        {
            Debug.Log("Exiting Patrol State");
        }
        public override void UpdateState()
        {
            if (PlayerInViewDistance()) _enemyStateMachine.ChangeState(_enemy.FollowState);

            float distance = Vector2.Distance(_enemy.Rigidbody.position,
            (Vector2)(_currentTarget != null ? _currentTarget.position : _enemy.PatrolPoints[_currentPatrolPointIndex].position));

            if (distance <= 1f)
            {
                _currentPatrolPointIndex++;
                if (_currentPatrolPointIndex >= _enemy.PatrolPoints.Count) _currentPatrolPointIndex = 0;
                _currentTarget = _enemy.PatrolPoints[_currentPatrolPointIndex];
            }

            if (_enemy.Seeker.IsDone()) _enemy.Seeker.StartPath(_enemy.Rigidbody.position, _currentTarget.position, OnPathComplete);
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
        }

        public override void AnimationTriggerEvent() { }

    }
}

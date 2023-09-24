
namespace Enjine
{
    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

        public override void EnterState()
        {
            Debug.Log("Entering Idle State");

            _player.PlayerAnimator.Play("Idle");
            _horizontalMove = 0f;
        }
        public override void ExitState()
        {
            base.ExitState();
        }
        public override void UpdateState()
        {
            base.UpdateState();

            if (IsMoving())
                _playerStateMachine.ChangeState(_player.WalkState);
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
        }
        public override void AnimationTriggerEvent(PlayerAnimationTrigger animationTrigger)
        {
            base.AnimationTriggerEvent(animationTrigger);
        }
    }
}

using UnityEngine;
namespace Enjine
{
    public class PlayerState
    {
        protected Player _player;
        protected PlayerStateMachine _playerStateMachine;

        protected float _horizontalMove;

        public PlayerState(Player player, PlayerStateMachine playerStateMachine)
        {
            _player = player;
            _playerStateMachine = playerStateMachine;
        }

        public virtual void EnterState() { }
        public virtual void ExitState() { }
        public virtual void UpdateState()
        {
            _horizontalMove = _player.InputControls.Player.Move.ReadValue<float>();
        }
        public virtual void FixedUpdateState()
        {
            _player.CharacterController.
            Move(_horizontalMove * Time.fixedDeltaTime * _player.Data.WalkSpeed, _player.IsCrouching, _player.IsJumping);

            _player.IsJumping = false;
        }
        public virtual void AnimationTriggerEvent(PlayerAnimationTrigger animationTrigger)
        {
            _player.PlayerAnimator.SetTrigger(animationTrigger.ToString());
        }

        protected bool IsMoving()
        {
            return _horizontalMove != 0 || _player.IsJumping || _player.IsCrouching || !_player.CharacterController.Grounded;
        }

    }
}

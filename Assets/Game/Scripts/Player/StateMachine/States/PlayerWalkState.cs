using UnityEngine;

public class PlayerWalkState : PlayerState
{
    private float _updateStateCooldown;

    public PlayerWalkState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void EnterState()
    {
        Debug.Log("Entering Walk State");

        _updateStateCooldown = 0;

        if (_player.CharacterController.Grounded && _player.IsJumping == false && _player.IsCrouching == false)
            _player.PlayerAnimator.Play("Walk");
    }
    public override void ExitState()
    {
        _updateStateCooldown = 0;
    }
    public override void UpdateState()
    {
        base.UpdateState();

        if (_player.CharacterController.Grounded && _player.PlayerAnimator.GetBool("Jump") && _player.IsJumping == false)
            _player.PlayerAnimator.SetBool("Jump", false);

        if (IsMoving() == false)
        {
            _updateStateCooldown += Time.deltaTime;

            if (_updateStateCooldown > .2f)
                _playerStateMachine.ChangeState(_player.IdleState);
        }
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }
    public override void AnimationTriggerEvent(PlayerAnimationTrigger animationTrigger)
    {
        if (animationTrigger == PlayerAnimationTrigger.Jump)
            _player.PlayerAnimator.SetBool("Jump", true);
        else base.AnimationTriggerEvent(animationTrigger);
    }
}

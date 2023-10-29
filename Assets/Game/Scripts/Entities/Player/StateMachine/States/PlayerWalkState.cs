using UnityEngine;
using FMOD.Studio;
using FMODUnity;

namespace Enjine
{
    public class PlayerWalkState : PlayerState
    {
        private float _updateStateCooldown;
        private EventInstance _playerFootsteps;

        public PlayerWalkState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
        {
            _playerFootsteps = AudioManager.Instance.CreateInstance(AudioDatabase.Instance.PlayerFootsteps);
        }

        public override void EnterState()
        {
            Debug.Log("Entering Walk State");

            _updateStateCooldown = 0;

            _playerFootsteps.setParameterByName("Footsteps Zone", (int)_player.CurrentAudioZone);

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

            CheckForFootstepsSound();

            if (IsMoving() && _player.CharacterController.Grounded)
            {
                PLAYBACK_STATE playbackState;
                _playerFootsteps.getPlaybackState(out playbackState);

                if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
                    _playerFootsteps.start();
            }
            else _playerFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        private void CheckForFootstepsSound()
        {
            if (_player.EnteredSoundTrigger == false) return;

            _playerFootsteps.setParameterByName("Footsteps Zone", (int)_player.CurrentAudioZone);

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
}

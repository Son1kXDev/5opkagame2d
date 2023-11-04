using UnityEngine;
using FMOD.Studio;
using FMODUnity;

namespace Enjine
{
    public class PlayerWalkState : PlayerState
    {

        private float _updateStateCooldown;
        private EventInstance _playerFootsteps;
        private EventInstance _playerJump;

        public PlayerWalkState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
        {
            _playerFootsteps = AudioManager.Instance.CreateInstance(AudioDatabase.Instance.PlayerFootstepsWalk);
        }

        //TODO: Create script for player audio management

        public override void EnterState()
        {
            Debug.Log("Entering Walk State");

            _updateStateCooldown = 0;

            _playerFootsteps.setParameterByName("Footsteps Zone", (int)_player.CurrentAudioZone);
            _playerFootsteps.start();

            if (_player.CharacterController.Grounded && _player.IsJumping == false && _player.IsCrouching == false)
                _player.PlayerAnimator.Play("Walk");

            _player.OnJumpEvent += OnJump;
            _player.OnCrouchEvent += OnCrouch;
            _player.OnRunEvent += OnRun;
        }
        public override void ExitState()
        {
            _updateStateCooldown = 0;
            _player.OnJumpEvent -= OnJump;
            _player.OnCrouchEvent -= OnCrouch;
            _player.OnRunEvent -= OnRun;
        }

        private void OnJump()
        {
            _playerFootsteps.setPaused(true);
            _playerJump = _player.CurrentAudioZone switch
            {
                AudioZone.Wood => AudioManager.Instance.CreateInstance(AudioDatabase.Instance.PlayerJumpWood),
                AudioZone.Stone => AudioManager.Instance.CreateInstance(AudioDatabase.Instance.PlayerJumpStone),
                _ => AudioManager.Instance.CreateInstance(AudioDatabase.Instance.PlayerJumpGrass),
            };
            _playerJump.start();
        }

        private void OnJumpEnd()
        {
            _playerJump.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _player.PlayerAnimator.SetBool("Jump", false);
        }

        private void OnCrouch(bool value)
        {
            _playerFootsteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _playerFootsteps = value
            ? AudioManager.Instance.CreateInstance(AudioDatabase.Instance.PlayerFootstepsCrouch)
            : AudioManager.Instance.CreateInstance(AudioDatabase.Instance.PlayerFootstepsWalk);
            _playerFootsteps.start();
        }

        private void OnRun(bool value)
        {
            _playerFootsteps = value
            ? AudioManager.Instance.CreateInstance(AudioDatabase.Instance.PlayerFootstepsRun)
            : AudioManager.Instance.CreateInstance(AudioDatabase.Instance.PlayerFootstepsWalk);
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if (_player.CharacterController.Grounded && _player.PlayerAnimator.GetBool("Jump") && _player.IsJumping == false)
                OnJumpEnd();

            if (IsMoving() == false)
            {
                _updateStateCooldown += Time.deltaTime;

                if (_updateStateCooldown > .2f)
                    _playerStateMachine.ChangeState(_player.IdleState);
            }

        }

        private void CheckForFootstepsSound()
        {
            if (_player.EnteredSoundTrigger == false) return;

            _playerFootsteps.setParameterByName("Footsteps Zone", (int)_player.CurrentAudioZone);
            _playerJump.setParameterByName("Footsteps Zone", (int)_player.CurrentAudioZone);
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();

            CheckForFootstepsSound();

            if (_horizontalMove != 0 && _player.CharacterController.Grounded)
            {
                _playerFootsteps.getPaused(out bool paused);
                if (paused) _playerFootsteps.setPaused(false);
            }
            else _playerFootsteps.setPaused(true);
        }
        public override void AnimationTriggerEvent(PlayerAnimationTrigger animationTrigger)
        {
            if (animationTrigger == PlayerAnimationTrigger.Jump)
                _player.PlayerAnimator.SetBool("Jump", true);
            else base.AnimationTriggerEvent(animationTrigger);
        }
    }
}

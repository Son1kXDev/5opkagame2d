using Enjine.Weapons;

namespace Enjine
{
    public class PlayerAttackState : PlayerState
    {
        private Weapon _weapon;

        public PlayerAttackState(Player player, PlayerStateMachine playerStateMachine, Weapon weapon) : base(player, playerStateMachine)
        {
            this._weapon = weapon;
            _weapon.OnExit += ExitHandler;
        }

        public override void EnterState()
        {
            Debug.Log("Entering Attack State");

            _weapon.Enter();
        }

        private void ExitHandler()
        {
            if (IsMoving()) _playerStateMachine.ChangeState(_player.WalkState);
            else _playerStateMachine.ChangeState(_player.IdleState);
        }

        public override void ExitState()
        {
            Debug.Log("Exiting Attack State");
            base.ExitState();
        }
    }
}

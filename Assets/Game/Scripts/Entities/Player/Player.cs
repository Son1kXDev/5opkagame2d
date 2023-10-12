using System;
using UnityEngine;
using Enjine.Weapons;
using Enjine.Data;

namespace Enjine
{
    public enum PlayerAnimationTrigger { Idle, Attack, Walk, Jump, Falling, Crouch, Dead }

    public class Player : Health, Data.IDataPersistence
    {
        public static Player Instance => _instance;
        private static Player _instance;

        [field: SerializeField, StatusIcon] public PlayerData Data { get; private set; }
        public InputControls InputControls { get; private set; }
        public CharacterController2D CharacterController { get; private set; }

        public PlayerStateMachine StateMachine { get; private set; }
        public PlayerIdleState IdleState { get; private set; }
        public PlayerWalkState WalkState { get; private set; }
        public PlayerAttackState AttackState { get; private set; }

        public bool IsCrouching { get; private set; }
        public bool IsJumping { get; set; }
        public Animator PlayerAnimator { get; private set; }

        private Weapon _primaryWeapon;
        private Weapon _secondaryWeapon;

        private void Awake()
        {
            if (_instance) Destroy(this.gameObject);
            else _instance = this;
        }

        private void OnValidate()
        {
            CharacterController ??= GetComponent<CharacterController2D>();
            _primaryWeapon ??= transform.Find("PrimaryWeapon").GetComponent<Weapon>();
            _secondaryWeapon ??= transform.Find("SecondaryWeapon").GetComponent<Weapon>();
        }

        private void Start() => InitializePlayer();

        public void InitializePlayer()
        {
            if (Data == null) throw new NullReferenceException("Player config is null");

            PlayerAnimator = GetComponentInChildren<Animator>();

            InputControls = new InputControls();
            InputControls.Player.Attack.performed += ctx => Attack();
            InputControls.Player.Crouch.performed += ctx => Crouch();
            InputControls.Player.Crouch.canceled += ctx => UnCrouch();
            InputControls.Player.Jump.performed += ctx => Jump();
            InputControls.Enable();

            StateMachine = new PlayerStateMachine();
            IdleState = new PlayerIdleState(this, StateMachine);
            WalkState = new PlayerWalkState(this, StateMachine);
            AttackState = new PlayerAttackState(this, StateMachine, _primaryWeapon);

            StateMachine.Initialize(IdleState);

            InitializeHealth(Data.MaximumHealth);
        }

        private void Update() => StateMachine.CurrentPlayerState.UpdateState();
        private void FixedUpdate() => StateMachine.CurrentPlayerState.FixedUpdateState();

        private void Jump()
        {
            if (CharacterController.Grounded && !IsCrouching)
            {
                IsJumping = true;
                StateMachine.CurrentPlayerState.AnimationTriggerEvent(PlayerAnimationTrigger.Jump);
            }
        }

        private void Crouch()
        {
            if (CharacterController.Grounded && !IsJumping)
            {
                IsCrouching = true;
                PlayerAnimator.SetBool("Crouch", true);
            }
        }

        private void UnCrouch()
        {
            IsCrouching = false;
            PlayerAnimator.SetBool("Crouch", false);
        }

        private void Attack()
        {
            StateMachine.ChangeState(AttackState);
        }

        public void SetInput(bool value)
        {
            if (value == false)
            {
                IsCrouching = false;
                IsJumping = false;
                CharacterController.Move(0, false, false);
                StateMachine.ChangeState(IdleState);
                InputControls.Disable();
            }
            else InputControls.Enable();
        }

        protected override void VisualizeHealth()
        {
            Debug.SetColor(Color.red);
            Debug.Log("TAKING DAMAGE! Current health: " + _currentHealth);
        }

        protected override void OnDeath()
        {
            Debug.Log("Player died!");
            SceneLoadManager.Instance.ReloadCurrentScene();
        }

        public void LoadData(object data)
        {
            GameData game = data as GameData;

            transform.position = game.CurrentPlayerPosition;
            Debug.Log("Nickname: " + game.NickName);
            Debug.Log("Current progress: " + game.CurrentProgress);
        }

        public void SaveData(object data)
        {
            GameData game = (GameData)data;
            game.CurrentPlayerPosition = transform.position;

            data = game;
        }
    }
}

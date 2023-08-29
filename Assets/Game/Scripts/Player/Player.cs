using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateEnum { Idle, Walk, Jump, Crouch, PreparingForDeath, Dead }

public enum PlayerAnimationTrigger { Idle, Attack, Walk, Jump, Falling, Crouch, Dead }

public class Player : Health
{
    public static Player Instance => _instance;
    private static Player _instance;

    public Action<PlayerStateEnum> OnPlayerStateUpdated;
    public PlayerStateEnum CurrentState => _currentState;
    private PlayerStateEnum _currentState;

    [field: SerializeField, StatusIcon] public PlayerConfig Config { get; private set; }
    public InputControls InputControls { get; private set; }
    public CharacterController2D CharacterController { get; private set; }

    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }

    public bool IsCrouching { get; private set; }
    public bool IsJumping { get; set; }

    public Animator PlayerAnimator { get; private set; }

    private PlayerAnimationController _animationController;
    private PlayerInputController _inputController;


    private void Awake()
    {
        if (_instance) Destroy(this.gameObject);
        else _instance = this;
    }

    private void Start() => InitializePlayer();

    public void InitializePlayer()
    {
        if (Config == null) throw new NullReferenceException("Player config is null");

        TryGetComponent<PlayerInputController>(out _inputController);
        TryGetComponent<PlayerAnimationController>(out _animationController);

        // _inputController?.Initialize();
        // _animationController?.Initialize();

        PlayerAnimator = GetComponentInChildren<Animator>();
        CharacterController = GetComponent<CharacterController2D>();

        InputControls = new InputControls();
        InputControls.Player.Attack.performed += ctx => Attack();
        InputControls.Player.Crouch.performed += ctx => Crouch();
        InputControls.Player.Crouch.canceled += ctx => UnCrouch();
        InputControls.Player.Jump.performed += ctx => Jump();
        InputControls.Enable();

        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        WalkState = new PlayerWalkState(this, StateMachine);

        StateMachine.Initialize(IdleState);

        InitializeHealth(Config.MaximumHealth);
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
        StateMachine.CurrentPlayerState.AnimationTriggerEvent(PlayerAnimationTrigger.Attack);
    }

    public void UpdatePlayerState(PlayerStateEnum newState)
    {
        if (newState == _currentState) return;
        _currentState = newState;
        OnPlayerStateUpdated?.Invoke(newState);
    }

    public void SetInput(bool value)
    {
        if (value == false)
        {
            IsCrouching = false;
            IsJumping = false;
            CharacterController.Move(0, false, false);
            StateMachine.ChangeState(IdleState);
        }

        _inputController.enabled = value;
    }

    protected override void VisualizeHealth()
    {
        Debug.SetColor(Color.red);
        Debug.Log("TAKING DAMAGE! Current health: " + _currentHealth);
    }

    protected override void OnDeath()
    {
        Debug.Log("Player died!");
    }
}

using UnityEngine;


[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(CharacterController2D))]
public class PlayerInputController : MonoBehaviour
{
    [SerializeField] float _walkSpeed = 10f;

    private InputControls _inputControls;
    private Player _controller;
    private CharacterController2D _characterController;
    private Rigidbody2D _rigidbody;
    private float _horizontalMove = 0f;
    private bool _isJumping = false;
    private bool _isCrouching = false;

    public void Initialize()
    {
        _controller = GetComponent<Player>();
        _characterController = GetComponent<CharacterController2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _inputControls = new InputControls();

        _inputControls.Player.Attack.performed += ctx => Attack();
        _inputControls.Player.Crouch.performed += ctx => Crouch();
        _inputControls.Player.Crouch.canceled += ctx => UnCrouch();
        _inputControls.Player.Jump.performed += ctx => Jump();

        _inputControls.Enable();
    }

    private void OnEnable() => _inputControls?.Enable();
    private void OnDisable() => _inputControls?.Disable();

    public void ForceStop()
    {
        _isCrouching = false;
        _isJumping = false;
        _horizontalMove = 0;
        _characterController.Move(0, false, false);
        _rigidbody.velocity = Vector2.zero;
        UpdateState();
    }

    private void Update()
    {
        _horizontalMove = _inputControls.Player.Move.ReadValue<float>();
        UpdateState();
    }

    private bool IsMoving() { return _horizontalMove != 0; }

    private void Attack() { }

    private void Crouch()
    {
        if (_characterController.Grounded && !_isJumping)
        {
            _isCrouching = true;
            _controller.UpdatePlayerState(PlayerStateEnum.Crouch);
        }
    }
    private void UnCrouch() => _isCrouching = false;
    private void Jump()
    {
        if (_characterController.Grounded && !_isCrouching)
        {
            _isJumping = true;
            _controller.UpdatePlayerState(PlayerStateEnum.Jump);
        }
    }

    //todo: переделать это в нормальный updatestate
    private void UpdateState()
    {
        if (IsMoving() && _characterController.Grounded && !_isCrouching && !_isJumping)
            _controller.UpdatePlayerState(PlayerStateEnum.Walk);

        if (!IsMoving() && _characterController.Grounded && !_isCrouching && !_isJumping)
            _controller.UpdatePlayerState(PlayerStateEnum.Idle);
    }

    private void FixedUpdate()
    {
        _characterController.Move(_horizontalMove * Time.fixedDeltaTime * _walkSpeed, _isCrouching, _isJumping);

        _isJumping = false;
    }
}
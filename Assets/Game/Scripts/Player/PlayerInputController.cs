using System;
using System.Collections;
using System.Collections.Generic;
using NTC.Global.Cache;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(CharacterController2D))]
public class PlayerInputController : MonoBehaviour
{
    [SerializeField] float _walkSpeed = 10f;

    private PlayerController _controller;
    private CharacterController2D _characterController;
    private Rigidbody2D _rigidbody;

    private float _horizontalMove = 0f;

    private bool _isJumping = false;
    private bool _isCrouching = false;

    public void Initialize()
    {
        _controller = GetComponent<PlayerController>();
        _characterController = GetComponent<CharacterController2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

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
        _horizontalMove = Input.GetAxisRaw("Horizontal");
        UpdateState();
    }

    //todo: проверить вне unity
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus == false && _controller != null && _characterController != null)
        { ForceStop(); }
    }

    private bool isMoving() { return _horizontalMove != 0; }

    //todo: переделать это в нормальный updatestate
    private void UpdateState()
    {

        if (Input.GetButtonDown("Jump") && _characterController.Grounded && !_isCrouching)
        {
            _isJumping = true;
            _controller.UpdatePlayerState(PlayerState.Jump);
            return;
        }

        if (Input.GetButtonDown("Crouch") && _characterController.Grounded && !_isJumping)
        {
            _isCrouching = true;
            _controller.UpdatePlayerState(PlayerState.Crouch);
            return;
        }
        else if (Input.GetButtonUp("Crouch")) _isCrouching = false;

        if (isMoving() && _characterController.Grounded && !_isCrouching && !_isJumping)
            _controller.UpdatePlayerState(PlayerState.Walk);


        if (!isMoving() && _characterController.Grounded && !_isCrouching && !_isJumping)
            _controller.UpdatePlayerState(PlayerState.Idle);

    }

    private void FixedUpdate()
    {
        _characterController.Move(_horizontalMove * Time.fixedDeltaTime * _walkSpeed, _isCrouching, _isJumping);

        _isJumping = false;
    }
}
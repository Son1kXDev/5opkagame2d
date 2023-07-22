using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(CharacterController2D))]
public class PlayerInputController : MonoBehaviour
{
    [SerializeField] float _walkSpeed = 10f;

    private PlayerController _controller;
    private CharacterController2D _characterController;

    private float _horizontalMove = 0f;

    private bool _isJumping = false;
    private bool _isCrouching = false;

    public void Initialize()
    {
        _controller = GetComponent<PlayerController>();
        _characterController = GetComponent<CharacterController2D>();
    }

    private void Update()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
            _isJumping = true;

        if (Input.GetButtonDown("Crouch")) _isCrouching = true;
        else if (Input.GetButtonUp("Crouch")) _isCrouching = false;
    }

    private void FixedUpdate()
    {
        _characterController.Move(_horizontalMove * Time.fixedDeltaTime * _walkSpeed, _isCrouching, _isJumping);

        _isJumping = false;
    }
}
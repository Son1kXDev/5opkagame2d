using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerAnimationController : MonoBehaviour
{

    private Animator _animator;
    private PlayerController _controller;

    public void Initialize()
    {
        _animator = GetComponentInChildren<Animator>();
        _controller = GetComponent<PlayerController>();

        _controller.OnPlayerStateUpdated += PlayerStateUpdated;
    }

    private void PlayerStateUpdated(PlayerState state)
    {
        _animator.SetTrigger(state.ToString());
    }
}
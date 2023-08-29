using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerAnimationController : MonoBehaviour
{

    private Animator _animator;
    private Player _controller;

    public void Initialize()
    {
        _animator = GetComponentInChildren<Animator>();
        _controller = GetComponent<Player>();

        _controller.OnPlayerStateUpdated += PlayerStateUpdated;
    }

    private void PlayerStateUpdated(PlayerStateEnum state)
    {
        _animator.Play(state.ToString());
    }
}
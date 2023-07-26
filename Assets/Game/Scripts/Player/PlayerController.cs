using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { Idle, Walk, Jump, Crouch, PreparingForDeath, Dead }

public class PlayerController : MonoBehaviour
{

    public Action<PlayerState> OnPlayerStateUpdated;

    public PlayerState CurrentState => _currentState;

    private PlayerState _currentState;
    private PlayerAnimationController _animationController;
    private PlayerInputController _inputController;

    private void Start() => InitializePlayer();

    public void InitializePlayer()
    {
        //todo: перенести установку компонентов в отдельный скрипт
        TryGetComponent<PlayerInputController>(out _inputController);
        TryGetComponent<PlayerAnimationController>(out _animationController);

        _inputController?.Initialize();
        _animationController?.Initialize();
    }

    public void UpdatePlayerState(PlayerState newState)
    {
        if (newState == _currentState) return;

        _currentState = newState;

        // Debug.Clear();
        // Debug.Log($"Current state: {newState}");

        OnPlayerStateUpdated?.Invoke(newState);
    }
}

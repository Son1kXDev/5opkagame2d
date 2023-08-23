using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { Idle, Walk, Jump, Crouch, PreparingForDeath, Dead }

public class PlayerController : Health
{
    public static PlayerController Instance => _instance;
    private static PlayerController _instance;

    public Action<PlayerState> OnPlayerStateUpdated;
    public PlayerState CurrentState => _currentState;

    private PlayerState _currentState;
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
        //todo: перенести установку компонентов в отдельный скрипт
        TryGetComponent<PlayerInputController>(out _inputController);
        TryGetComponent<PlayerAnimationController>(out _animationController);

        _inputController?.Initialize();
        _animationController?.Initialize();


        InitializeHealth();
    }

    public void UpdatePlayerState(PlayerState newState)
    {
        if (newState == _currentState) return;

        _currentState = newState;

        // Debug.Clear();
        // Debug.Log($"Current state: {newState}");

        OnPlayerStateUpdated?.Invoke(newState);
    }

    public void SetInput(bool value)
    {
        if (_inputController.enabled) _inputController.ForceStop();
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

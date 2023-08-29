using UnityEngine;


[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Resources/Game/Enemy Config", order = 0)]
public class EnemyConfig : ScriptableObject
{

    [field: Header("Enemy Stats")]
    [field: SerializeField, StatusIcon(minValue: 0)] public int MaximumHealth { get; private set; } = 100;
    [field: SerializeField, StatusIcon(minValue: 0)] public int Damage { get; private set; } = 10;
    [field: SerializeField, StatusIcon(minValue: 0f)] public float Speed { get; private set; } = 200f;
    [field: SerializeField, StatusIcon(minValue: 0f)] public float JumpForce { get; private set; } = 0.3f;

    [field: Header("Pathfinding System")]
    [field: SerializeField, StatusIcon(minValue: 0f)] public float ActivationDistance { get; private set; } = 8.5f;
    [field: SerializeField, StatusIcon(minValue: 0f)] public float PathUpdateSeconds { get; private set; } = .1f;

    [field: Header("Follow System")]
    [field: SerializeField, StatusIcon(minValue: -0.1f)] public float MemoryDelay { get; private set; } = 1.5f;

    [field: Header("Attack System")]
    [field: SerializeField, StatusIcon(minValue: 0f)] public float AttackDistance { get; private set; } = 1.5f;
    [field: SerializeField, StatusIcon(minValue: 0f)] public float AttackCooldown { get; private set; } = 2.5f;

    [field: Header("Physics")]
    [field: SerializeField, StatusIcon(minValue: 0f)] public float NextWaypointDistance { get; private set; } = 3f;
    [field: SerializeField, StatusIcon(minValue: 0f)] public float JumpNodeHeightRequirement { get; private set; } = 0.8f;
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }

    [field: Header("Custom Behaviour")]
    [field: SerializeField] public bool JumpEnabled { get; private set; } = true;
    [field: SerializeField] public bool DirectionLookEnabled { get; private set; } = true;

}

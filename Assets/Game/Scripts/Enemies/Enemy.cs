using UnityEngine;


[CreateAssetMenu(fileName = "Enemy", menuName = "Resources/Game/Enemy", order = 0)]
public class Enemy : ScriptableObject
{

    [field: Header("Pathfinding System")]
    [field: SerializeField, StatusIcon(minValue: 0f)] public float ActivationDistance { get; private set; } = 8.5f;
    [field: SerializeField, StatusIcon(minValue: 0f)] public float PathUpdateSeconds { get; private set; } = .1f;

    [field: Header("Attack System")]
    [field: SerializeField, StatusIcon(minValue: 0f)] public float AttackDistance { get; private set; } = 1.5f;
    [field: SerializeField, StatusIcon(minValue: 0f)] public float AttackCooldown { get; private set; } = 2.5f;
    [field: SerializeField, StatusIcon(minValue: 0)] public int Damage { get; private set; } = 10;

    [field: Header("Physics")]
    [field: SerializeField, StatusIcon(minValue: 0f)] public float Speed { get; private set; } = 200f;
    [field: SerializeField, StatusIcon(minValue: 0f)] public float NextWaypointDistance { get; private set; } = 3f;
    [field: SerializeField, StatusIcon(minValue: 0f)] public float JumpNodeHeightRequirement { get; private set; } = 0.8f;
    [field: SerializeField, StatusIcon(minValue: 0f)] public float JumpModifier { get; private set; } = 0.3f;
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }

    [field: Header("Custom Behaviour")]
    [field: SerializeField] public bool JumpEnabled { get; private set; } = true;
    [field: SerializeField] public bool DirectionLookEnabled { get; private set; } = true;

}

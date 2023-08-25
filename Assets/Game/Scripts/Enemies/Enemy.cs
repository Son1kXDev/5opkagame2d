using UnityEngine;


[CreateAssetMenu(fileName = "Enemy", menuName = "Resources/Game/Enemy", order = 0)]
public class Enemy : ScriptableObject
{

    [field: Header("Field of view")]
    [field: SerializeField] public float FieldVerticalRange { get; private set; } = 8.5f;
    [field: SerializeField] public float FieldHorizontalRange { get; private set; } = 5.5f;
    [field: SerializeField] public float FieldColliderDistance { get; private set; } = 0.6f;

    [field: Header("Attack")]
    [field: SerializeField] public float AttackVerticalRange { get; private set; } = 3f;
    [field: SerializeField] public float AttackHorizontalRange { get; private set; } = 2f;
    [field: SerializeField] public float AttackColliderDistance { get; private set; } = 0.5f;
    [field: SerializeField] public float AttackDelay { get; private set; } = 1.5f;
    [field: SerializeField] public int Damage { get; private set; } = 10;

}

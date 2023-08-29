using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Resources/Game/Player Config", order = 0)]
public class PlayerConfig : ScriptableObject
{
    [field: SerializeField, StatusIcon(minValue: 0)] public int MaximumHealth { get; private set; } = 100;
    [field: SerializeField, StatusIcon(minValue: 0f)] public float WalkSpeed { get; private set; } = 20f;
}

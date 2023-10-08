using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enjine
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Resources/Game/Player Data", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [field: SerializeField, StatusIcon(minValue: 0)] public int MaximumHealth { get; private set; } = 100;
        [field: SerializeField, StatusIcon(minValue: 0f)] public float WalkSpeed { get; private set; } = 20f;

        [field: SerializeField, StatusIcon] public Material[] HairMaterials {get; private set; }
    }
}
using System;
using UnityEngine;

namespace Enjine.Weapons.Components
{
    [Serializable]
    public class ActionHitBoxData : ComponentData<AttackActionHitBox>
    {
        [field: SerializeField] public LayerMask DetectableLayers { get; private set; }
    }
}

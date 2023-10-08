using System;
using UnityEngine;

namespace Enjine.Weapons.Components
{
    [Serializable]
    public class AttackDamage : AttackData
    {
        [field: SerializeField] public int Amount { get; private set; }
    }
}

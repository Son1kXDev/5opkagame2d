using System;
using UnityEngine;

namespace Enjine.Weapons.Components
{
    public class AttackData
    {
        [SerializeField, HideInInspector] private string name;

        public void SetAttackName(int i) => name = $"Attack {i}";

        public virtual void SetPhasesNames() { }
    }
}

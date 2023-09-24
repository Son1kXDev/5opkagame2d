using System;
using UnityEngine;

namespace Enjine.Weapons.Components
{
    [Serializable]
    public class AttackSprites : AttackData
    {
        [field: SerializeField] public PhaseSprites[] PhaseSprites { get; private set; }

        public override void SetPhasesNames()
        {
            base.SetPhasesNames();
            for (int i = 0; i < PhaseSprites.Length; i++)
                PhaseSprites[i].Name = PhaseSprites[i].Phase.ToString();
        }
    }

    [Serializable]
    public struct PhaseSprites
    {
        [HideInInspector] public string Name;
        [field: SerializeField] public AttackPhases Phase { get; private set; }
        [field: SerializeField] public Sprite[] Sprites { get; private set; }

    }
}

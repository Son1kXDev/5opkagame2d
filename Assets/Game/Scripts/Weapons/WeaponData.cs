using System;
using System.Collections.Generic;
using System.Linq;
using Enjine.Weapons.Components;
using UnityEngine;

namespace Enjine.Weapons
{

    [CreateAssetMenu(fileName = "newWeaponData", menuName = "Resources/Game/Weapon Data", order = 2)]
    public class WeaponData : ScriptableObject
    {
        [field: SerializeField] public int NumberOfAttacks { get; private set; }
        [field: SerializeReference] public List<ComponentData> ComponentsData { get; private set; }

        public T GetData<T>()
        {
            return ComponentsData.OfType<T>().FirstOrDefault();
        }

        public void AddData(ComponentData data)
        {
            if (ComponentsData.FirstOrDefault(t => t.GetType() == data.GetType()) == null)
                ComponentsData.Add(data);
        }
    }
}

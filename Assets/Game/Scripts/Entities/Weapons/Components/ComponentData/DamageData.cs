using System;

namespace Enjine.Weapons.Components
{
    public class DamageData : ComponentData<AttackDamage>
    {
        public DamageData()
        {
            ComponentDependency = typeof(Damage);
        }
    }
}

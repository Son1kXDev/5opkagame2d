using UnityEngine;

namespace Enjine.Weapons.Components
{
    public class WeaponSpriteData : ComponentData<AttackSprites>
    {
        public WeaponSpriteData()
        {
            ComponentDependency = typeof(WeaponSprite);
        }
    }
}

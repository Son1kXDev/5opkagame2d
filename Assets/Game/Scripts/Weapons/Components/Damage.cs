using System;
using UnityEngine;

namespace Enjine.Weapons.Components
{
    public class Damage : WeaponComponent<DamageData, AttackDamage>
    {
        private ActionHitBox _hitBox;

        private void HandleDetectCollider2D(Collider2D[] colliders)
        {
            foreach (var item in colliders)
                if (item.TryGetComponent(out Health component))
                    component.TakeDamage(_currentAttackData.Amount);
        }
        protected override void Awake()
        {
            base.Awake();

            _hitBox = GetComponent<ActionHitBox>();
        }

        protected override void Start()
        {
            base.Start();
            _hitBox.OnDetectedCollider2D += HandleDetectCollider2D;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _hitBox.OnDetectedCollider2D -= HandleDetectCollider2D;
        }
    }
}

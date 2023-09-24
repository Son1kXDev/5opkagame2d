using System;
using UnityEngine;

namespace Enjine.Weapons.Components
{
    public class ActionHitBox : WeaponComponent<ActionHitBoxData, AttackActionHitBox>
    {
        public event Action<Collider2D[]> OnDetectedCollider2D;
        private Vector2 _offset;
        private Collider2D[] _detected;

        private void HandleAttackAction()
        {
            //TODO: Fix FacingDirection
            _offset.Set(
                transform.position.x + (_currentAttackData.HitBox.center.x * Player.Instance.CharacterController.FacingDirection()),
                transform.position.y + _currentAttackData.HitBox.center.y
            );

            _detected = Physics2D.OverlapBoxAll(_offset, _currentAttackData.HitBox.size, 0f, _data.DetectableLayers);

            if (_detected.Length != 0)
                // TakeDamage();
                OnDetectedCollider2D?.Invoke(_detected);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            EventHandler.OnAttackAction += HandleAttackAction;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventHandler.OnAttackAction -= HandleAttackAction;
        }

        private void OnDrawGizmos()
        {
            if (_data == null) return;

            foreach (var item in _data.AttackData)
            {
                if (item.Debug)
                    Gizmos.DrawWireCube(transform.position + (Vector3)item.HitBox.center, item.HitBox.size);
            }
        }
    }
}

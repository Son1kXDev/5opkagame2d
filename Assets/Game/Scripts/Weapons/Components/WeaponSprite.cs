using System;
using UnityEngine;
using Enjine.Weapons.Components;

namespace Enjine.Weapons.Components
{
    public class WeaponSprite : WeaponComponent<WeaponSpriteData, AttackSprites>
    {
        private SpriteRenderer _baseSpriteRenderer;
        private SpriteRenderer _weaponSpriteRenderer;
        private int _currentWeaponSpriteIndex;


        protected override void Awake()
        {
            base.Awake();

            _baseSpriteRenderer = transform.Find("Base").GetComponent<SpriteRenderer>();
            _weaponSpriteRenderer = transform.Find("WeaponSprite").GetComponent<SpriteRenderer>();
            // _baseSpriteRenderer = _weapon.BaseGameObject.GetComponent<SpriteRenderer>();
            // _weaponSpriteRenderer = _weapon.WeaponGameObject.GetComponent<SpriteRenderer>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _baseSpriteRenderer.RegisterSpriteChangeCallback(HandleBaseSpriteChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _baseSpriteRenderer.UnregisterSpriteChangeCallback(HandleBaseSpriteChange);
        }

        protected override void HandleEnter()
        {
            base.HandleEnter();
            _currentWeaponSpriteIndex = 0;
        }

        private void HandleBaseSpriteChange(SpriteRenderer sr)
        {
            if (!isAttackActive)
            {
                _weaponSpriteRenderer.sprite = null;
                return;
            }

            _weaponSpriteRenderer.sprite = _currentAttackData.Sprites[_currentWeaponSpriteIndex];
            _currentWeaponSpriteIndex++;
        }

    }

}

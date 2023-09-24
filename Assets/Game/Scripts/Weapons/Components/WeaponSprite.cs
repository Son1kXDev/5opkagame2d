using System;
using UnityEngine;
using Enjine.Weapons.Components;
using System.Linq;

namespace Enjine.Weapons.Components
{
    public class WeaponSprite : WeaponComponent<WeaponSpriteData, AttackSprites>
    {
        private SpriteRenderer _baseSpriteRenderer;
        private SpriteRenderer _weaponSpriteRenderer;
        private int _currentWeaponSpriteIndex;
        private Sprite[] _currentPhaseSprites;

        protected override void Start()
        {
            base.Start();

            _baseSpriteRenderer = _weapon.BaseGameObject.GetComponent<SpriteRenderer>();
            _weaponSpriteRenderer = _weapon.WeaponGameObject.GetComponent<SpriteRenderer>();

            _baseSpriteRenderer.RegisterSpriteChangeCallback(HandleBaseSpriteChange);
            EventHandler.OnEnterAttackPhase += HandleEnterAttackPhase;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _baseSpriteRenderer.UnregisterSpriteChangeCallback(HandleBaseSpriteChange);
            EventHandler.OnEnterAttackPhase -= HandleEnterAttackPhase;
        }

        protected override void HandleEnter()
        {
            base.HandleEnter();
            _currentWeaponSpriteIndex = 0;
        }

        private void HandleEnterAttackPhase(AttackPhases phase)
        {
            _currentWeaponSpriteIndex = 0;
            _currentPhaseSprites = _currentAttackData.PhaseSprites.FirstOrDefault(data => data.Phase == phase).Sprites;
        }

        private void HandleBaseSpriteChange(SpriteRenderer sr)
        {
            if (!isAttackActive)
            {
                _weaponSpriteRenderer.sprite = null;
                return;
            }

            _weaponSpriteRenderer.sprite = _currentPhaseSprites[_currentWeaponSpriteIndex];
            _currentWeaponSpriteIndex++;
        }

    }

}

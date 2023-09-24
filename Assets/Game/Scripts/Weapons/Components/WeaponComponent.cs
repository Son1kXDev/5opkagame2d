using Enjine.Weapons.Components;
using UnityEngine;

namespace Enjine.Weapons.Components
{
    public abstract class WeaponComponent : MonoBehaviour
    {
        protected Weapon _weapon;
        protected AnimationEventHandler EventHandler => _weapon.EventHandler;
        protected bool isAttackActive;

        protected virtual void Awake()
        {
            _weapon = GetComponent<Weapon>();
        }

        protected virtual void HandleEnter()
        {
            isAttackActive = true;
        }

        protected virtual void HandleExit()
        {
            isAttackActive = false;
        }

        protected virtual void OnEnable()
        {
            _weapon.OnEnter += HandleEnter;
            _weapon.OnExit += HandleExit;
        }

        protected virtual void OnDisable()
        {
            _weapon.OnEnter -= HandleEnter;
            _weapon.OnExit -= HandleExit;
        }
    }

    public abstract class WeaponComponent<T1, T2> : WeaponComponent
    where T1 : ComponentData<T2>
    where T2 : AttackData
    {
        protected T1 _data;
        protected T2 _currentAttackData;

        protected override void HandleEnter()
        {
            base.HandleEnter();

            _currentAttackData = _data.AttackData[0];
        }

        protected override void Awake()
        {
            base.Awake();

            _data = _weapon.Data.GetData<T1>();
        }
    }
}

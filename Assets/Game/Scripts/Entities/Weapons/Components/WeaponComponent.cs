using Enjine.Weapons.Components;
using UnityEngine;

namespace Enjine.Weapons.Components
{
    public abstract class WeaponComponent : MonoBehaviour
    {
        protected Weapon _weapon;
        protected AnimationEventHandler EventHandler => _weapon.EventHandler;
        protected bool isAttackActive;

        public virtual void Initialize() { }

        protected virtual void Awake()
        {
            _weapon = GetComponent<Weapon>();
        }

        protected virtual void Start()
        {
            _weapon.OnEnter += HandleEnter;
            _weapon.OnExit += HandleExit;
        }

        protected virtual void OnDestroy()
        {
            _weapon.OnEnter -= HandleEnter;
            _weapon.OnExit -= HandleExit;
        }

        protected virtual void HandleEnter()
        {
            isAttackActive = true;
        }

        protected virtual void HandleExit()
        {
            isAttackActive = false;
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

        public override void Initialize()
        {
            base.Initialize();

            _data = _weapon.Data.GetData<T1>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Enjine.Weapons.Components;
using UnityEngine;

namespace Enjine.Weapons
{
    public class WeaponGenerator : MonoBehaviour
    {
        [SerializeField, StatusIcon] private WeaponData _data;
        private List<WeaponComponent> _componentsAlreadyOnWeapon = new List<WeaponComponent>();
        private List<WeaponComponent> _componentsAddedToWeapon = new List<WeaponComponent>();
        private List<Type> _componentDependencies = new List<Type>();
        private Weapon _weapon;
        private Animator _animator;

        private void Awake()
        {
            _weapon = GetComponent<Weapon>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start() => GenerateWeapon(_data);

        public void GenerateWeapon(WeaponData data)
        {
            _weapon.SetData(data);

            _componentsAlreadyOnWeapon.Clear();
            _componentsAddedToWeapon.Clear();
            _componentDependencies.Clear();

            _componentsAlreadyOnWeapon = GetComponents<WeaponComponent>().ToList();
            _componentDependencies = data.GetAllDependencies();

            foreach (var dependency in _componentDependencies)
            {
                if (_componentsAddedToWeapon.FirstOrDefault(component => component.GetType() == dependency))
                    continue;

                var weaponComponent = _componentsAlreadyOnWeapon.FirstOrDefault(component => component.GetType() == dependency);

                if (weaponComponent == null) weaponComponent = gameObject.AddComponent(dependency) as WeaponComponent;

                weaponComponent.Initialize();

                _componentsAddedToWeapon.Add(weaponComponent);
            }

            var componentsToRemove = _componentsAlreadyOnWeapon.Except(_componentsAddedToWeapon);

            foreach (var weaponComponent in componentsToRemove)
                Destroy(weaponComponent);

            _animator.runtimeAnimatorController = _data.AnimatorController;
        }
    }

}

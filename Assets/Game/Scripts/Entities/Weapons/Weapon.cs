using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enjine.Weapons
{
    public class Weapon : MonoBehaviour
    {
        public event Action OnEnter;
        public event Action OnExit;

        public WeaponData Data { get; private set; }
        public GameObject BaseGameObject { get; private set; }
        public GameObject WeaponGameObject { get; private set; }
        public AnimationEventHandler EventHandler { get; private set; }
        private Animator _animator;

        private void OnValidate()
        {
            BaseGameObject ??= transform.Find("Base").gameObject;
            WeaponGameObject ??= transform.Find("WeaponSprite").gameObject;

            if (BaseGameObject == null) return;

            EventHandler ??= BaseGameObject.GetComponent<AnimationEventHandler>();
            _animator ??= BaseGameObject.GetComponent<Animator>();
        }

        public void SetData(WeaponData data) => Data = data;


        public void Enter()
        {
            Debug.Log($"{transform.name} entered");

            BaseGameObject ??= transform.Find("Base").gameObject;
            WeaponGameObject ??= transform.Find("WeaponSprite").gameObject;
            EventHandler ??= BaseGameObject.GetComponent<AnimationEventHandler>();
            _animator ??= BaseGameObject.GetComponent<Animator>();

            EventHandler.OnFinished += Exit;

            _animator.SetBool("active", true);
            OnEnter?.Invoke();
        }

        private void Exit()
        {
            _animator.SetBool("active", false);
            OnExit?.Invoke();
            EventHandler.OnFinished -= Exit;
        }
    }
}
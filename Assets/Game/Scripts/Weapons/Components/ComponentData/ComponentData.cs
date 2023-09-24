using System;
using System.Linq;
using UnityEngine;

namespace Enjine.Weapons.Components
{
    [Serializable]
    public class ComponentData
    {
        [SerializeField, HideInInspector] private string name;

        public Type ComponentDependency { get; protected set; }

        public ComponentData()
        {
            SetComponentName();
        }

        public void SetComponentName() => name = GetType().Name;

        public virtual void SetAttackDataNames() { }
        public virtual void SetAttackDataPhasesNames() { }
        public virtual void InitializeAttackData(int numberOfAttacks) { }
    }

    [Serializable]
    public class ComponentData<T> : ComponentData where T : AttackData
    {
        [SerializeField] private T[] _attackData = new T[] { };
        public T[] AttackData { get => _attackData; private set => _attackData = value; }

        public override void SetAttackDataNames()
        {
            base.SetAttackDataNames();
            for (int i = 0; i < AttackData.Length; i++)
                AttackData[i].SetAttackName(i + 1);
        }
        public override void SetAttackDataPhasesNames()
        {
            base.SetAttackDataPhasesNames();

            for (int i = 0; i < AttackData.Length; i++)
                AttackData[i].SetPhasesNames();
        }
        public override void InitializeAttackData(int numberOfAttacks)
        {
            base.InitializeAttackData(numberOfAttacks);

            var oldLength = AttackData.Length;

            if (oldLength == numberOfAttacks) return;

            Array.Resize(ref _attackData, numberOfAttacks);

            if (oldLength < numberOfAttacks)
            {
                for (int i = oldLength; i < AttackData.Length; i++)
                {
                    var newObj = Activator.CreateInstance(typeof(T)) as T;
                    AttackData[i] = newObj;
                }
            }

            SetAttackDataNames();
            SetAttackDataPhasesNames();
        }
    }
}

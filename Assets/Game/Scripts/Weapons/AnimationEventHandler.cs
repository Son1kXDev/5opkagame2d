using System;
using UnityEngine;

namespace Enjine.Weapons
{
    public class AnimationEventHandler : MonoBehaviour
    {
        public event Action OnFinished;
        public event Action OnAttackAction;
        public event Action<AttackPhases> OnEnterAttackPhase;

        private void AnimationFinishedTrigger() => OnFinished?.Invoke();
        private void AttackActionTrigger() => OnAttackAction?.Invoke();
        private void EnterAttackPhase(AttackPhases phase) => OnEnterAttackPhase?.Invoke(phase);

    }
}

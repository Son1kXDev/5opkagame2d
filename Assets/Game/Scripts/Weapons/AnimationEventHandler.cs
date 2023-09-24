using System;
using UnityEngine;

namespace Enjine.Weapons
{
    public class AnimationEventHandler : MonoBehaviour
    {
        public event Action OnFinished;
        public event Action OnAttackAction;

        private void AnimationFinishedTrigger() => OnFinished?.Invoke();
        private void AttackActionTrigger() => OnAttackAction?.Invoke();

    }
}

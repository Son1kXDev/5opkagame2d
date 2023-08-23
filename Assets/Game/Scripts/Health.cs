using System.Collections;
using System.Collections.Generic;
using NTC.Global.Cache;
using UnityEngine;

public abstract class Health : MonoCache
{
    [SerializeField, StatusIcon(minValue: 0)] protected int _maxHealth;
    protected int _currentHealth;

    private bool _dead = false;

    /// <summary>
    /// Resets the health to the max health.
    /// </summary>
    protected virtual void InitializeHealth()
    {
        _currentHealth = _maxHealth;
        _dead = false;
    }

    /// <summary>
    /// Takes away the entity's health in the amount of damage.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(int damage)
    {
        if (_dead) return;

        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
        VisualizeHealth();

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _dead = true;
            OnDeath();
        }

    }

    /// <summary>
    /// Adds the health entity to the amount of the healing value.
    /// </summary>
    /// <param name="healAmount"></param>
    public virtual void TakeHeal(int healAmount)
    {
        if (_dead) return;

        _currentHealth = Mathf.Clamp(_currentHealth + healAmount, 0, _maxHealth);
        VisualizeHealth();
    }

    protected abstract void VisualizeHealth();

    protected abstract void OnDeath();

}

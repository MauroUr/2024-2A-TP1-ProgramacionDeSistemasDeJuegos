using HealthSystem;
using System;
using UnityEngine.UI;

public class StructureHealthDecorator : Health
{
    private Slider healthBar;
    public StructureHealthDecorator(int maxHp, Slider slider) : base(maxHp)
    {
        healthBar = slider;
    }

    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints);
        if (healthBar != null)
            healthBar.value = _currentHp;
    }

    public override void Revive()
    {
        base.Revive();
        if (healthBar != null)
            healthBar.value = _currentHp;
    }
}

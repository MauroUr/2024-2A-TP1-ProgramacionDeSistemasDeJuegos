using HealthSystem;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Structure : MonoBehaviour, IHealth
{
    public Health health;
    [SerializeField] private Slider healthBar;

    private void Awake()
    {
        health = new StructureHealthDecorator(100, healthBar);
    }
    private void OnEnable()
    {
        //health.OnDeath += this.OnDeath.Invoke;
        health.OnDeath += Destroyed;
    }
    private void OnDisable()
    {
        //health.OnDeath -= this.OnDeath.Invoke;
        health.OnDeath -= Destroyed;
    }
    
    private void Destroyed() => this.gameObject.SetActive(false);
    
    public void TakeDamage(int damage) => health.TakeDamage(damage);

    public void Revive() => health.Revive();

    public void Kill() => health.Kill();

}

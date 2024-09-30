using System;

namespace HealthSystem
{
    public class Health : IHealth
    {
        protected int _maxHP;
        protected int _currentHp;

        public event Action OnDeath = delegate { };        

        public Health(int maxHp)
        {
            _maxHP = maxHp;
            _currentHp = maxHp;
        }

        public virtual void TakeDamage(int damagePoints)
        {
            if (damagePoints < 0)
                Console.WriteLine($"El daño fue {damagePoints}. Por favor verificar este resultado no esperado.");

            _currentHp -= damagePoints;
            
            if (_currentHp <= 0)
                OnDeath?.Invoke();
        }

        public void Kill() => TakeDamage(_maxHP);

        public virtual void Revive() => _currentHp = _maxHP;
    }

    public interface IHealth
    {
        public void TakeDamage(int damagePoints);

        public void Kill();

        public void Revive();
    }
}

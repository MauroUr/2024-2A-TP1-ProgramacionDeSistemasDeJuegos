using HealthSystem;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour, IHealth
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private EnemiesParameters parameters;
        
        private GameObject _townCenter;

        public event Action OnSpawn = delegate { };
        public Health health;

        private void Reset() => FetchComponents();

        private void Awake() => FetchComponents();

        private void FetchComponents()
        {
            agent ??= GetComponent<NavMeshAgent>();
            health = new Health(parameters.maxHealth);
        }
        private void OnEnable()
        {
            //health.OnDeath += this.OnDeath.Invoke;
            health.OnDeath += Die;
        }

        private void OnDisable()
        {
            //health.OnDeath -= this.OnDeath.Invoke;
            health.OnDeath -= Die;
        }

        public void SetDestination(GameObject townCenter)
        {
            if (townCenter == null)
            {
                Debug.LogError($"{name}: Found no {nameof(townCenter)}!! :(");
                return;
            }

            _townCenter = townCenter;
            Vector3 destination = townCenter.transform.position;
            destination.y = transform.position.y;
            agent.SetDestination(destination);
            OnSpawn();
        }

        private void Update()
        {
            if (agent.hasPath && Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
            {
                Debug.Log($"{name}: I'll die for my people!");
                health.Kill();
            }
            else if (!agent.hasPath)
                Destroy(this.gameObject);
        }

        private void Die()
        {
            _townCenter.GetComponent<Structure>().TakeDamage(parameters.explosionDmg);
            this.gameObject.SetActive(false);
        }

        public void TakeDamage(int damage) => health.TakeDamage(damage);

        public void Revive() => health.Revive();

        public void Kill() => health.Kill();
    }

    [CreateAssetMenu(fileName = "EnemiesParameters", menuName = "Enemies/EnemiesParameters")]
    public class EnemiesParameters : ScriptableObject
    {
        [SerializeField] private int _explosionDmg;
        [SerializeField] private int _maxHealth;

        public int explosionDmg => _explosionDmg;
        public int maxHealth => _maxHealth;

    }
}

using System;
using System.Collections;
using UnityEngine;
using Enemies;
using System.Collections.Generic;
using System.Linq;
using Audio;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private SpawnerParameters parameters;

    EnemyManager enemyManager;
    
    private void Awake()
    {
        StartCoroutine(GetEnemyManager());
    }

    private IEnumerator Start()
    {
        while (!destroyCancellationToken.IsCancellationRequested)
        {
            List<Structure> targets = ServiceLocator.instance.GetService<StructureService>(typeof(StructureService)).GetAllStructures();

            if (enemyManager == null || !enemyManager.ready || !targets.Any(target => target.gameObject.activeSelf))
                yield return null;

            for (int i = 0; i < parameters.spawnsPerPeriod; i++)
            {
                Enemy enemy = enemyManager.enemyPool.FindInactive();
                if (enemy == null)
                {
                    enemy = Instantiate(characterPrefab, transform.position, transform.rotation).GetComponent<Enemy>();
                    enemyManager.enemyPool.Register(enemy);
                    enemy.health.OnDeath += () => enemyManager.enemyPool.MarkAsInactive(enemy);
                    EnemyManager.instance.SetTarget(enemy);
                }
                else
                {
                    enemy.transform.position = transform.position;
                    enemy.transform.rotation = transform.rotation;
                    enemyManager.enemyPool.MarkAsActive(enemy);
                    enemy.gameObject.SetActive(true);
                }
            }
            yield return new WaitForSeconds(parameters.period);
        }
    }

    private IEnumerator GetEnemyManager()
    {
        while (ServiceLocator.instance == null)
            yield return null;

        enemyManager = ServiceLocator.instance.GetService<EnemyManager>(typeof(EnemyManager));
    }
}

[CreateAssetMenu(fileName = "SpawnerParameters", menuName = "Spawner/SpawnerParameters")]
public class SpawnerParameters : ScriptableObject
{
    [SerializeField] private int _spawnsPerPeriod;
    [SerializeField] private float _frequency;
    [SerializeField] private float _period;

    public int spawnsPerPeriod => _spawnsPerPeriod;
    public float frequency => _frequency;

    public float period => _period;

    private void OnEnable()
    {
        if (_frequency > 0)
            _period = 1f / _frequency; 
        
    }
}


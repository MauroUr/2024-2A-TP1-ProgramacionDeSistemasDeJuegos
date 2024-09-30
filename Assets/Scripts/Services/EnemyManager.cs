using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance { get; private set; }

    private List<Structure> targets;

    public bool ready = false;

    public ObjectPool<Enemy> enemyPool;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        enemyPool = new ObjectPool<Enemy>();
        StartCoroutine(SuscribeService());
        StartCoroutine(GetStructures());
    }

    private void OnDisable()
    {
        //enemyPool.Clear();
    }
    private IEnumerator SuscribeService()
    {
        while (ServiceLocator.instance == null)
            yield return null;

        ServiceLocator.instance.SetService(typeof(EnemyManager), this);
        
    }
    private IEnumerator GetStructures()
    {
        while (ServiceLocator.instance == null)
            yield return null;
        yield return null;
        targets = ServiceLocator.instance.GetService<StructureService>(typeof(StructureService)).GetAllStructures();
        ready = true;
    }

    public void SetTarget(Enemy enemy)
    {
        GameObject target = targets[0].gameObject;

        foreach (Structure structure in targets)
        {
            if (!structure.gameObject.activeSelf) continue;

            float distanceToStructure = Vector3.Distance(target.transform.position, enemy.transform.position);

            if (Vector3.Distance(structure.gameObject.transform.position, enemy.transform.position) < distanceToStructure)
                target = structure.gameObject;
        }

        enemy.SetDestination(target);
    }
}



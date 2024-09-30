using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureService : MonoBehaviour
{
    [SerializeField] private List<Structure> structures;
    public static StructureService instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        StartCoroutine(SuscribeService());
    }

    private IEnumerator SuscribeService()
    {
        while (ServiceLocator.instance == null)
            yield return null;

        ServiceLocator.instance.SetService(typeof(StructureService), this);

        yield return null;
        foreach (Structure structure in structures)
            structure.health.OnDeath += () => StartCoroutine(ReappearStructure(structure));
    }
    private void OnDisable()
    {
        foreach (Structure structure in structures)
            structure.health.OnDeath -= () => StartCoroutine(ReappearStructure(structure));
    }

    public List<Structure> GetAllStructures()
    {
        return structures;
    }

    private IEnumerator ReappearStructure(Structure structure)
    {
        yield return new WaitForSecondsRealtime(10f);

        structure.gameObject.SetActive(true);
        structure.Revive();
    }
}

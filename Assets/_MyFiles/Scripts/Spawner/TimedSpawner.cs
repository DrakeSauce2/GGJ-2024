using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpawner : MonoBehaviour
{
    [Header("Base Spawner")]
    [SerializeField] bool startOnAwake = false;
    [SerializeField] List<GameObject> spawnList = new List<GameObject>();
    [SerializeField, Range(0, 5)] int spawnAmount = 1;
    [SerializeField] float spawnDelay = 1f;
    private float currentDelay = 0;
    [Space]
    [SerializeField] int minSpawn = 1, maxSpawn = 3;

    [Header("Ringmaster Fight")]
    [SerializeField] GameObject ringmasterMinion;

    Coroutine spawnCoroutine = null;

    private void Start()
    {
        if (startOnAwake == true) 
            StartSpawnCycle();
    }

    public void StopSpawnCycle()
    {
        if (spawnCoroutine == null) return;

        StopCoroutine(spawnCoroutine);
    }

    public void StartSpawnCycle()
    {
        spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    public void SetSpawnAmountRange(int min, int max)
    {
        minSpawn = min;
        maxSpawn = max;
    }

    private GameObject GetRandObjectInList()
    {
        int rand = Random.Range(0, spawnList.Count);
        return spawnList[rand];
    }

    private IEnumerator SpawnCoroutine()
    {
        for (int i = spawnAmount; i > 0; i--)
        {
            GameManager.Instance.AddEnemyToList(Instantiate(GetRandObjectInList(), transform.position, Quaternion.identity));
        }
        currentDelay = spawnDelay;
        while (currentDelay > 0)
        {
            currentDelay -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        spawnAmount = Random.Range(minSpawn, maxSpawn);

        spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    public List<GameObject> Spawn()
    {
        List<GameObject> instancedObjects = new List<GameObject>();
        spawnAmount = Random.Range(minSpawn, maxSpawn);

        Debug.Log("Spawning Minions");
        instancedObjects.Add(Instantiate(ringmasterMinion, transform.position, Quaternion.identity));

        return instancedObjects;
    }

    public void ForceSpawn()
    {
        for (int i = spawnAmount; i > 0; i--)
        {
            GameManager.Instance.AddEnemyToList(Instantiate(GetRandObjectInList(), transform.position, Quaternion.identity));
        }

        spawnAmount = Random.Range(minSpawn, maxSpawn);
    }
    public void InstantDelay()
    {
        currentDelay = 0;
    }

}

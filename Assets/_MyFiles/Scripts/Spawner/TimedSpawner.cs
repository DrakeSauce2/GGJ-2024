using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpawner : MonoBehaviour
{
    [Header("Base Spawner")]
    [SerializeField] List<GameObject> spawnList = new List<GameObject>();
    [SerializeField, Range(1, 5)] int spawnAmount = 1;
    [SerializeField] float spawnDelay = 1f;

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
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
            GameManager.Instance.AddInstanceToSpawnList(Instantiate(GetRandObjectInList(), transform.position, Quaternion.identity));
        }
        yield return new WaitForSeconds(spawnDelay);

        StartCoroutine(SpawnCoroutine());
    }

}

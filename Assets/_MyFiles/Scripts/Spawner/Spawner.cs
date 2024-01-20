using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Base Spawner")]
    [SerializeField] List<GameObject> spawnList = new List<GameObject>();
    [SerializeField, Range(1, 5)] int spawnAmount = 1;

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        for (int i = spawnAmount; i > 0; i--)
        {
            GameManager.Instance.AddInstanceToSpawnList(Instantiate(GetRandObjectInList(), transform));
        }
    }

    private GameObject GetRandObjectInList()
    {
        int rand = Random.Range(0, spawnList.Count);
        return spawnList[rand];
    }

}

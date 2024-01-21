using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Base Spawner")]
    [SerializeField] List<GameObject> spawnList = new List<GameObject>();
    [SerializeField, Range(1, 5)] int spawnAmount = 1;

    public void Spawn()
    {
        for (int i = spawnAmount; i > 0; i--)
        {
            Instantiate(GetRandObjectInList(), transform.position, Quaternion.identity);
        }
    }

    public void RemoteManualSpawn(GameObject selectedObjSpawn)
    {
        Instantiate(selectedObjSpawn, transform.position, Quaternion.identity);
    }

    private GameObject GetRandObjectInList()
    {
        int rand = Random.Range(0, spawnList.Count);
        return spawnList[rand];
    }

}

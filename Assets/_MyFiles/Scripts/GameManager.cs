using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] List<GameObject> instancedSpawns = new List<GameObject>();
    [SerializeField] TimedSpawner enemySpawner;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
    }

    private void LateUpdate()
    {
        if (instancedSpawns.Count == 0)
        {
            enemySpawner.ForceSpawn();
        }
    }

    public void AddEnemyToList(GameObject EnemyToAdd)
    {
        instancedSpawns.Add(EnemyToAdd);
    }

    public void RemoveEnemyFromList(GameObject EnemyToRemove)
    {
        instancedSpawns.Remove(EnemyToRemove);
    }

}

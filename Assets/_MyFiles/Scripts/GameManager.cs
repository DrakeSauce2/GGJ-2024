using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] List<GameObject> instancedSpawns = new List<GameObject>();

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
    }

    public void AddInstanceToSpawnList(GameObject objToAdd)
    {
        instancedSpawns.Add(objToAdd);
    }

    public void RemoveSpawnedInstanced(GameObject objToRemove)
    {
        instancedSpawns.Remove(objToRemove);
        Destroy(objToRemove);
    }

}

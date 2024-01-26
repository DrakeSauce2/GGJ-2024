using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    [Header("Enemy")]
    [SerializeField] List<GameObject> instancedSpawns = new List<GameObject>();
    [SerializeField] List<TimedSpawner> enemySpawner;
    [Space]

    [SerializeField] Transform spotlight;
    public Transform spotLight { get { return spotlight; } }

    [SerializeField] GameObject endDoor;

    [SerializeField] int minSpawnersToUse = 1, maxSpawnersToUse = 3;

    [Header("Curtains")]
    [SerializeField] Animator curtainAnimator;
    [SerializeField] GameObject exitCollider;

    [Header("UI")]
    [SerializeField] GameObject mainUI;

    private bool forceSpawnStarted = false;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);

        Time.timeScale = 1.0f;

        Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
    }

    public void SpawnEndDoor()
    {
        endDoor.SetActive(true);
        exitCollider.SetActive(false);
    }

    public void StartEnemySpawnCycle()
    {
        foreach (TimedSpawner spawner in enemySpawner)
        {
            spawner.StartSpawnCycle();
        }
    }
    public void StopEnemySpawnCycle()
    {
        foreach (TimedSpawner spawner in enemySpawner)
        {
            spawner.StopSpawnCycle();
        }
    }

    public void KillAllInstancedSpawns()
    {
        if (instancedSpawns.Count <= 0) return;

        foreach (GameObject objectToKill in instancedSpawns)
        {
            Destroy(objectToKill);
        }
    }

    public void OpenCurtain()
    {
        if(curtainAnimator != null)
        {
            curtainAnimator.SetTrigger("Open");
        }
    }

    private void LateUpdate()
    {
        if (instancedSpawns.Count == 0)
        {
            if (forceSpawnStarted == true) return;

            StartCoroutine(StartForceSpawn());
        }
    }

    public void AddEnemyToList(GameObject EnemyToAdd)
    {
        instancedSpawns.Add(EnemyToAdd);
    }

    public void RemoveEnemyFromList(GameObject EnemyToRemove)
    {
        if (instancedSpawns.Contains(EnemyToRemove))
        {
            instancedSpawns.Remove(EnemyToRemove);
        }
    }

    private IEnumerator StartForceSpawn()
    {
        forceSpawnStarted = true;

        yield return new WaitForSeconds(4f);

        for (int i = 0; i < Random.Range(minSpawnersToUse, maxSpawnersToUse); i++)
        {
            int randSpawner = Random.Range(0, enemySpawner.Count);

            enemySpawner[randSpawner].InstantDelay();
        }

        forceSpawnStarted = false;
    }

    public void InstantForceSpawn()
    {
        for (int i = Random.Range(minSpawnersToUse, maxSpawnersToUse); i > 0; i--)
        {
            int randSpawner = Random.Range(0, enemySpawner.Count);

            enemySpawner[randSpawner].ForceSpawn();
        }
    }
    public void InstantDelay()
    {
        for (int i = 0; i < Random.Range(minSpawnersToUse, maxSpawnersToUse); i++)
        {
            int randSpawner = Random.Range(0, enemySpawner.Count);

            enemySpawner[randSpawner].InstantDelay();
        }
    }

    public void DisableMainUI()
    {
        mainUI.SetActive(false);
    }

}

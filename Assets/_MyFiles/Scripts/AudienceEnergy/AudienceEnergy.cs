using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceEnergy : MonoBehaviour
{
    public static AudienceEnergy Instance;

    [Header("Spawners")]
    [SerializeField] private List<Spawner> spawners = new List<Spawner>();
    [SerializeField] private List<GameObject> goodObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> badObjects = new List<GameObject>();
    [Header("Audience Energy")]
    [SerializeField] private float energy = 50f;
    [SerializeField] private float energyDecayRate = 0.1f;
    [SerializeField] private float energyHitGain = 4f;
    private float spawnRate = 4f;
    Coroutine spawnCoroutine = null;
    [Space]
    [SerializeField] private float energyDecayStallTimeGain = 0.3f;
    [SerializeField] private float totalStallTime = 0f;
    [SerializeField] bool isStalled = false;
    Coroutine energyStallCoroutine = null;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);

        spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    private void Update()
    {
        EnergyDecay();
    }

    private IEnumerator SpawnCoroutine()
    {
        int spawnAmount = Random.Range(1, 3);
        for (int i = spawnAmount; i > 0; i--)
        {
            int randSpawner = Random.Range(0, spawners.Count);
            spawners[randSpawner].RemoteManualSpawn(GetRandObjectInList(GetListBasedOnEnergyCurrent()));
        }
        yield return new WaitForSeconds(spawnRate);

        spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    private List<GameObject> GetListBasedOnEnergyCurrent()
    {       
        if (energy < 20)
        {
            spawnRate = 2f;
            energyDecayRate = 0.5f;
            return badObjects;
        }
        else if(energy < 50 && energy > 20)
        {
            spawnRate = 4f;
            energyDecayRate = 1f;
            return badObjects;
        }
        else if (energy >= 50 && energy < 80)
        {
            spawnRate = 4f;
            energyDecayRate = 5f;
            return goodObjects;
        }
        else
        {
            spawnRate = 2f;
            energyDecayRate = 10f;
            return goodObjects;
        }

    }

    private GameObject GetRandObjectInList(List<GameObject> spawnList)
    {
        int rand = Random.Range(0, spawnList.Count);
        return spawnList[rand];
    }

    private void EnergyDecay()
    {
        if (isStalled) return;

        energy -= energyDecayRate * Time.deltaTime;

        if (energy < 0)
        {
            energy = 0;
        }
    }

    public void GainEnergy()
    {
        energy += energyHitGain;
        totalStallTime += energyDecayStallTimeGain;

        if(energy > 100)
            energy = 100;

        if (energyStallCoroutine != null)
        {
            StopCoroutine(energyStallCoroutine);
            isStalled = false;
        }

        energyStallCoroutine = StartCoroutine(StallEnergyDecayCoroutine());
    }

    private IEnumerator StallEnergyDecayCoroutine()
    {
        while (totalStallTime > 0)
        {
            totalStallTime -= Time.deltaTime;
            isStalled = true;

            yield return new WaitForEndOfFrame();
        }

        isStalled = false;
    }

}

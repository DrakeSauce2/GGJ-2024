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
    private int spawnMin = 2, spawnMax = 4;
    private float spawnRate = 4f;
    Coroutine spawnCoroutine = null;
    [Space]
    [SerializeField] private float energyDecayStallTimeGain = 0.3f;
    [SerializeField] private float totalStallTime = 0f;
    [SerializeField] bool isStalled = false;

    private bool overrideEnergyMeter = false;

    Coroutine energyStallCoroutine = null;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);

        StartCoroutine(StartDelayCoroutine());
    }

    public IEnumerator StartDelayCoroutine()
    {
        yield return new WaitForSeconds(3f);

        spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    private void Update()
    {
        EnergyDecay();
    }

    public void StopSpawningCycle()
    {
        StopCoroutine(spawnCoroutine);
    }

    public void StartSpawnCoroutine()
    {
        spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        int spawnAmount = Random.Range(spawnMin, spawnMax);
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
        if (overrideEnergyMeter == true)
        {
            spawnRate = .6f;
            energyDecayRate = 0f;
            spawnMin = 4;
            spawnMax = 6;
            return badObjects;
        }


        if (energy < 20)
        {
            spawnRate = 2f;
            energyDecayRate = 0.5f;
            spawnMin = 4;
            spawnMax = 8;
            return badObjects;
        }
        else if(energy < 50 && energy > 20)
        {
            spawnRate = 4f;
            energyDecayRate = 1f;
            spawnMin = 2;
            spawnMax = 4;
            return badObjects;
        }
        else if (energy >= 50 && energy < 80)
        {
            spawnRate = 4f;
            energyDecayRate = 5f;
            spawnMin = 1;
            spawnMax = 4;
            return goodObjects;
        }
        else
        {
            spawnRate = 2f;
            energyDecayRate = 10f;
            spawnMin = 4;
            spawnMax = 8;

            return goodObjects;
        }

    }

    public void SetOverrideEnergyMeter(bool state)
    {
        overrideEnergyMeter = state;
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

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
    [Space]
    [SerializeField] private float energyDecayStallTime = 0.5f;
    [SerializeField] bool isStalled = false;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Update()
    {
        if (isStalled) return;

        EnergyDecay();
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

    private void EnergyDecay()
    {
        energy -= energyDecayRate * Time.deltaTime;
    }

    private IEnumerator StallEnergyDecayCoroutine()
    {
        isStalled = true;

        yield return new WaitForSeconds(energyDecayStallTime);

        isStalled = false;
    }

}

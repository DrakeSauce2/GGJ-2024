using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RingMaster : Character
{
    public static RingMaster Instance;

    NavMeshAgent agent;
    private Damager damager;

    [Header("Ring Master Health GUI")]
    [SerializeField] RingMasterHealthGUI healthGUI;

    [Header("Move Point")]
    [SerializeField] int phaseCount = 0;
    [SerializeField] Transform[] movePoints;
    [SerializeField] Transform[] damagePoints;

    [Header("Hats")]
    [SerializeField] GameObject realHat;
    [SerializeField] GameObject animHat;

    [Header("Look At Point")]
    [SerializeField] Transform lookAtPoint;

    Coroutine actionCoroutine;

    private bool isAttacking = false;
    private bool isDead = false;
    [SerializeField] private bool isMoving = false;

    [Header("Damage Phase")]
    [SerializeField] bool damagePhase = false;
    private int damageDoneInThisPhase = 0;

    [Header("Minion Spawn")]
    [SerializeField] List<TimedSpawner> spawners = new List<TimedSpawner>();
    [SerializeField] List<GameObject> instancedMinions = new List<GameObject>();


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        Init(gameObject);

        damager = GetComponent<Damager>();
        agent = GetComponent<NavMeshAgent>();

        healthGUI.Init(Health, MaxHealth);
        healthComponent.onDeath += StartDeath;

    }

    private void Start()
    {
        StartDamagePhase();
    }
    public void RemoveMinionFromList(GameObject minionToRemove)
    {
        instancedMinions.Remove(minionToRemove);
    }
    public override void TakeDamage(GameObject instigator, int damage, int team)
    {
        if (damagePhase == false) return;
        if (Team == team) return;

        Health -= damage;
        damageDoneInThisPhase += damage;

        Debug.Log(Health + " : " + MaxHealth);

        healthComponent.onHealthChanged?.Invoke(instigator, Health, MaxHealth);
        healthGUI.onHealthChanged?.Invoke(Health);

        if (damageDoneInThisPhase > 10)
        {
            EndOfDamagePhase();
        }

    }

    private void EndOfDamagePhase()
    {
        for (int i = Random.Range(1, 5); i > 0; i--)
        {
            int randSpawner = Random.Range(0, 4);
            instancedMinions.AddRange(spawners[randSpawner].Spawn());
        }

        damagePhase = false;
        damageDoneInThisPhase = 0;

        StartCoroutine(MoveToPoint(movePoints[phaseCount]));

        phaseCount++;
    }
    public void StartDamagePhase()
    {
        GameManager.Instance.KillAllInstancedSpawns();

        StartCoroutine(MoveToPoint(damagePoints[phaseCount]));

        damagePhase = true;
    }

    private void StartDeath()
    {
        isDead = true;

        GameManager.Instance.StopEnemySpawnCycle();
        GameManager.Instance.KillAllInstancedSpawns();
        AudienceEnergy.Instance.StopSpawningCycle();

        realHat.SetActive(false);
        animHat.SetActive(true);

    }

    private void Update()
    {
        if (isAttacking || isDead || isMoving) return;

        if (instancedMinions.Count <= 0 && damagePhase == false)
        {
            StartDamagePhase();
        }

        switch (Random.Range(1, 4)) 
        {
            case 1:
                Debug.Log("Ringmaster Attack!");
                actionCoroutine = StartCoroutine(AttackCoroutine());
                break;
            case 2:
                Debug.Log("Ringmaster Summon Enemies!");
                actionCoroutine = StartCoroutine(SummonEnemies());
                break;
            case 3:
                Debug.Log("Ringmaster Anger Crowd!");
                actionCoroutine = StartCoroutine(AngerCrowd());
                break;
            default:
                Debug.Log("Ringmaster state default!");
                break;
        }


    }

    #region Coroutines

    public IEnumerator AttackCoroutine()
    {
        if (agent == null) yield return null;

        isAttacking = true;

        // Attack Stuff and animation here
        damager.StartDamage(1f);

        yield return new WaitForSeconds(1.5f); // Arbitrary time until animations are implemented
        isAttacking = false;
    }

    public IEnumerator SummonEnemies()
    {
        isAttacking = true;
        GameManager.Instance.InstantForceSpawn();

        yield return new WaitForSeconds(5f);
        isAttacking = false;
    }

    public IEnumerator AngerCrowd()
    {
        isAttacking = true;
        AudienceEnergy.Instance.SetOverrideEnergyMeter(true);

        yield return new WaitForSeconds(2f);

        isAttacking = false;
        AudienceEnergy.Instance.SetOverrideEnergyMeter(false);
    }

    private IEnumerator MoveToPoint(Transform point)
    {
        isMoving = true;

        agent.SetDestination(point.position);

        float distance = Vector3.Distance(transform.position, point.position);
        while (distance > 1f)
        {
            distance = Vector3.Distance(transform.position, point.position);

            Debug.Log("Ringmaster Moving!");

            yield return new WaitForEndOfFrame();
        }

        float t = 2f;
        while (t > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, point.rotation, Time.deltaTime * 5f);

            t -=  Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        isMoving = false;
    }

    #endregion  

}

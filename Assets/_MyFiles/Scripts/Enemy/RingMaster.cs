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

    [Header("Look At Point")]
    [SerializeField] Transform lookAtPoint;

    Coroutine actionCoroutine;

    private bool isAttacking = false;
    [SerializeField] private bool isMoving = false;

    [Header("Damage Phase")]
    [SerializeField] bool damagePhase = false;
    private int damageDoneInThisPhase = 0;

    [Header("Minion Spawn")]
    [SerializeField] List<TimedSpawner> spawners = new List<TimedSpawner>();
    [SerializeField] List<GameObject> instancedMinions = new List<GameObject>();

    [Header("Death Cam")]
    [SerializeField] Camera deathCam;
    public Camera DeathCamera { get { return deathCam; } }

    [Header("Spotlight")]
    [SerializeField] private Transform spotLight;
    private Vector3 refVel;
    [SerializeField, Range(0, 10)] float followSpeed;
    [SerializeField] float spotlightHeight = 10f;
    [SerializeField] Color nonDamageColor;
    [SerializeField] Color damageColor;


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
        if (isDead) return;
        if (damagePhase == false) return;
        if (Team == team) return;

        Health -= damage;
        damageDoneInThisPhase += damage;

        Debug.Log(Health + " : " + MaxHealth);

        healthComponent.onHealthChanged?.Invoke(instigator, Health, MaxHealth);
        healthGUI.onHealthChanged?.Invoke(Health);

        PlaySoundClip(gruntAudio);

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

        spotLight.GetComponent<Light>().color = nonDamageColor;

        StartCoroutine(MoveToPoint(movePoints[phaseCount]));

        phaseCount++;
    }
    public void StartDamagePhase()
    {
        spotLight.GetComponent<Light>().color = damageColor;

        GameManager.Instance.KillAllInstancedSpawns();

        StartCoroutine(MoveToPoint(damagePoints[phaseCount]));

    }

    private void StartDeath()
    {
        isDead = true;
        _Animation.SetBool("isDead", isDead);

        GameManager.Instance.StopEnemySpawnCycle();
        GameManager.Instance.KillAllInstancedSpawns();
        AudienceEnergy.Instance.StopSpawningCycle();

        healthGUI.gameObject.SetActive(false);

        StartCoroutine(CameraManager.Instance.ShowRingmasterDeathCam());

    }

    private void FixedUpdate()
    {
        if (agent != null)
            _Animation.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void Update()
    {
        if (spotLight)
        {
            Vector3 target = new Vector3(transform.position.x, spotlightHeight, transform.position.z);

            spotLight.position = Vector3.SmoothDamp(spotLight.position, target, ref refVel, followSpeed);
        }

        if (damagePhase) return;

        if (isAttacking || isDead || isMoving) return;

        if (instancedMinions.Count <= 0 && damagePhase == false)
        {
            damagePhase = true;

            if(actionCoroutine != null)
                StopCoroutine(actionCoroutine);

            StartDamagePhase();
            return;
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
        if (isMoving) yield return null;


        isAttacking = true;
        _Animation.SetTrigger("Attack2");

        // Attack Stuff and animation here
        damager.StartDamage(1f);

        yield return new WaitForSeconds(3); // Arbitrary time until animations are implemented

        _Animation.ResetTrigger("Attack2");
        isAttacking = false;
    }

    public IEnumerator SummonEnemies()
    {
        if (isMoving) yield return null;

        isAttacking = true;

        _Animation.SetTrigger("Attack1");
        GameManager.Instance.InstantForceSpawn();

        yield return new WaitForSeconds(5f);

        _Animation.ResetTrigger("Attack1");
        isAttacking = false;
    }

    public IEnumerator AngerCrowd()
    {
        if (isMoving) yield return null;

        isAttacking = true;
        AudienceEnergy.Instance.SetOverrideEnergyMeter(true);
        _Animation.SetTrigger("Attack1");

        yield return new WaitForSeconds(3f);

        _Animation.ResetTrigger("Attack1");
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
            _Animation.SetFloat("Speed", agent.velocity.normalized.magnitude);

            Debug.Log("Ringmaster Moving!");

            yield return new WaitForEndOfFrame();
        }

        float t = 1f;
        while (t > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, point.rotation, Time.deltaTime * 5f);

            t -=  Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(.3f);

        isMoving = false;
    }

    #endregion  

}

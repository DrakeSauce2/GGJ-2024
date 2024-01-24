using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RingMaster : Character
{
    NavMeshAgent agent;
    private Damager damager;

    [Header("Ring Master Health GUI")]
    [SerializeField] RingMasterHealthGUI healthGUI;

    [Header("Move Point")]
    [SerializeField] Transform movePoint;

    Coroutine actionCoroutine;

    private bool isAttacking = false;
    private bool isDead = false;
    [SerializeField] private bool isMoving = false;

    [SerializeField] bool damagePhase = false;

    private void Awake()
    {
        Init(gameObject);

        damager = GetComponent<Damager>();
        agent = GetComponent<NavMeshAgent>();

        healthGUI.Init(Health, MaxHealth);

        StartCoroutine(MoveToPoint(movePoint.position));
    }

    public override void TakeDamage(GameObject instigator, int damage, int team)
    {
        if (damagePhase == true) return;
        if (Team == team) return;

        Health -= damage;

        Debug.Log(Health + " : " + MaxHealth);

        healthComponent.onHealthChanged?.Invoke(instigator, Health, MaxHealth);
        healthGUI.onHealthChanged?.Invoke(Health);
    }

    public void StartDamagePhase()
    {
        GameManager.Instance.KillAllInstancedSpawns();

        damagePhase = false;
    }

    private void StartDeath()
    {
        isDead = true;

    }

    private void FixedUpdate()
    {
        if (isAttacking || isDead || isMoving) return;

        if (damagePhase == true)
        {
            actionCoroutine = StartCoroutine(AttackCoroutine());

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
                Debug.Log("Ringmaster state defualt!");
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

        yield return new WaitForSeconds(5f);

        isAttacking = false;
        AudienceEnergy.Instance.SetOverrideEnergyMeter(false);
    }

    private IEnumerator MoveToPoint(Vector3 point)
    {
        isMoving = true;

        agent.SetDestination(movePoint.position);

        float distance = Vector3.Distance(transform.position, point);
        while (distance > 1f)
        {
            distance = Vector3.Distance(transform.position, point);

            Debug.Log("Ringmaster Moving!");

            yield return new WaitForEndOfFrame();
        }

        isMoving = false;
    }

    #endregion  

}

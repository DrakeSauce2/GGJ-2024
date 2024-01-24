using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    protected NavMeshAgent agent;
    private Damager damager;

    [SerializeField] protected RagdollEnabler ragdoll;

    Coroutine attackCoroutine;
    Coroutine stunCoroutine;


    bool isAttacking = false;
    protected bool isDead = false;
    bool isStunned = false;

    private void Awake()
    {
        Init(gameObject);

        damager = GetComponent<Damager>();
        agent = GetComponent<NavMeshAgent>();
 
        agent.speed = UnityEngine.Random.Range(agent.speed - 2f, agent.speed);

        healthComponent.onDeath -= StartDeath;
        healthComponent.onHealthChanged -= DamageTaken;
        healthComponent.onDeath += StartDeath;
        healthComponent.onHealthChanged += DamageTaken;
    }

    private void DamageTaken(GameObject instigator, int health, int maxHealth)
    {
        stunCoroutine = StartCoroutine(StunCoroutine());
    }

    private IEnumerator StunCoroutine()
    {
        if (agent == null) yield return null;

        isStunned = true;

        if (agent != null)
            agent.SetDestination(transform.position);

        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        yield return new WaitForSeconds(1.3f);

        isStunned = false;

        if (agent != null)
            agent.SetDestination(Player.Instance.transform.position);
    }

    public virtual void StartDeath()
    {
        isDead = true;

        GameManager.Instance.RemoveEnemyFromList(gameObject);

        StopAllCoroutines();

        Destroy(agent);

        ragdoll.EnableRagdoll();
        ragdoll.ApplyForce();

        gameObject.layer = 7;

        Destroy(gameObject, 3);

    }

    private void FixedUpdate()
    {
        if (isAttacking || isDead || isStunned) return;

        float distanceFromPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);

        if (distanceFromPlayer <= agent.stoppingDistance)
        {
            attackCoroutine = StartCoroutine(AttackCoroutine());
        }
        else
        {
            Debug.Log("Following Player!");
            agent.SetDestination(Player.Instance.transform.position);
        }

    }

    public IEnumerator AttackCoroutine()
    {
        if (agent == null) yield return null;

        isAttacking = true;

        // Attack Stuff and animation here
        Debug.Log("Enemy Attacking!");
        damager.StartDamage(1f);

        yield return new WaitForSeconds(1.5f); // Arbitrary time until animations are implemented
        isAttacking = false;

        agent.SetDestination(Player.Instance.transform.position);
    }

}

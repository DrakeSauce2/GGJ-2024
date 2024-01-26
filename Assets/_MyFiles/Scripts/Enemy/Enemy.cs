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

        agent.isStopped = true;

        _Animation.StopPlayback();
        _Animation.ResetTrigger("Attack");

        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        yield return new WaitForSeconds(1.3f);

        isStunned = false;
        agent.isStopped = false;

        if (agent != null)
            agent.SetDestination(Player.Instance.transform.position);
    }

    public virtual void StartDeath()
    {
        isDead = true;
        gameObject.layer = 7;

        if (stunCoroutine != null)
            StopCoroutine(stunCoroutine);

        if(attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        agent.ResetPath();
        agent.isStopped = true;
        agent.updatePosition = false;

        GameManager.Instance.RemoveEnemyFromList(gameObject);

        ragdoll.EnableRagdoll();
        ragdoll.ApplyForce();

        PlayDeathSound();

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
        _Animation.SetTrigger("Attack");

        yield return new WaitForSeconds(1.5f); 

        _Animation.ResetTrigger("Attack");

        isAttacking = false;

        agent.SetDestination(Player.Instance.transform.position);
    }

    public void Damage(float duration)
    {
        damager.StartDamage(duration);
    }

}

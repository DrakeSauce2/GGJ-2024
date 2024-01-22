using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    NavMeshAgent agent;
    private Damager damager;

    [SerializeField] float deathForce = 110f;


    Coroutine attackCoroutine;
    Coroutine stunCoroutine;


    bool isAttacking = false;
    bool isDead = false;
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
        agent.SetDestination(transform.position);

        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        yield return new WaitForSeconds(1.3f);

        isStunned = false;
        agent.SetDestination(Player.Instance.transform.position);
    }

    private void StartDeath()
    {
        isDead = true;

        GameManager.Instance.RemoveEnemyFromList(gameObject);

        StopAllCoroutines();

        Destroy(agent);

        Rigidbody rbody = gameObject.AddComponent<Rigidbody>();
        if(rbody != null)
        {
            rbody.AddForce(-transform.forward * deathForce);
        }

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

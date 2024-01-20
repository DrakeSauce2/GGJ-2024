using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Character
{
    NavMeshAgent agent;
    private Damager damager;

    bool isAttacking = false;
    bool isDead = false;

    private void Awake()
    {
        Init(gameObject);

        damager = GetComponent<Damager>();
        agent = GetComponent<NavMeshAgent>();

        healthComponent.onDeath += StartDeath;
    }

    private void StartDeath()
    {
        isDead = true;

        Destroy(gameObject);

    }

    private void FixedUpdate()
    {
        if (isAttacking || isDead) return;

        float distanceFromPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);

        if (distanceFromPlayer <= agent.stoppingDistance)
        {
            StartCoroutine(AttackCoroutine());
        }
        else
        {
            Debug.Log("Following Player!");
            agent.SetDestination(Player.Instance.transform.position);
        }

    }

    public IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        // Attack Stuff and animation here
        Debug.Log("Enemy Attacking!");
        damager.StartDamage(1f);

        yield return new WaitForSeconds(1.5f); // Arbitrary time until animations are implemented
        isAttacking = false;

        agent.SetDestination(Player.Instance.transform.position);
    }

}

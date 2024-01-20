using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Character
{
    NavMeshAgent agent;

    bool isAttacking = false;

    private void Awake()
    {
        Init(gameObject);

        agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        if (isAttacking) return;

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

        yield return new WaitForSeconds(1.5f); // Arbitrary time until animations are implemented
        isAttacking = false;

        agent.SetDestination(Player.Instance.transform.position);
    }

}

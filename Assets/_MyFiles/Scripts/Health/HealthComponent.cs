using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public GameObject Owner { get; private set; }

    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [Space]
    [SerializeField] private float invulnerableDuration;

    public delegate void OnDamageTaken(GameObject instigator, int damage);
    public delegate void OnDeath();

    public OnDamageTaken onDamageTaken;
    public OnDeath onDeath;

    private bool isDamagable = true;

    private void Awake()
    {
        onDamageTaken += TakeDamage;
    }

    public void Init(GameObject owner)
    {
        Owner = owner;
    }

    private void TakeDamage(GameObject instigator, int damage)
    {
        if (Owner == instigator) return;
        if (!isDamagable) return;

        Debug.Log("Damage Taken");

        health -= damage;
        StartCoroutine(InvulnerabeCoroutine());

        if (health <= 0)
        {
            onDeath?.Invoke();
        }
    }

    private IEnumerator InvulnerabeCoroutine()
    {
        isDamagable = false;

        yield return new WaitForSeconds(invulnerableDuration);

        isDamagable = true;
    }

}

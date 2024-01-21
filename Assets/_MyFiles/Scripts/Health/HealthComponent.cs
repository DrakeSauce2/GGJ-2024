using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public GameObject Owner { get; private set; }

    [Space]
    [SerializeField] private float invulnerableDuration;

    public delegate void OnHealthChanged(GameObject instigator, int damage, int health, int maxHealth);
    public delegate void OnDeath();

    public OnHealthChanged onHealthChanged;
    public OnDeath onDeath;

    private bool isDamagable = true;

    private void Awake()
    {
        onHealthChanged += TakeDamage;
    }

    public void Init(GameObject owner)
    {
        Owner = owner;
    }

    private void TakeDamage(GameObject instigator, int damage, int health, int maxHealth)
    {
        if (Owner == instigator) return;
        if (!isDamagable) return;

        Debug.Log("Damage Taken");
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public GameObject Owner { get; private set; }

    [SerializeField] private int health;
    [SerializeField] private int maxHealth;

    public delegate void OnDamageTaken(GameObject instigator, int damage);
    public OnDamageTaken onDamageTaken;

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

        health -= damage;
        if (health <= 0)
        {
            // Start Death
        }

    }
}

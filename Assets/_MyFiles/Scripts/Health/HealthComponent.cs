using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public GameObject Owner { get; private set; }

    public delegate void OnHealthChanged(GameObject instigator, int health, int maxHealth);
    public delegate void OnDeath();

    public OnHealthChanged onHealthChanged;
    public OnDeath onDeath;

    private bool isDamagable = true;

    private void Awake()
    {
        onHealthChanged += HealthChanged;
    }

    public void Init(GameObject owner)
    {
        Owner = owner;
    }

    private void HealthChanged(GameObject instigator, int health, int maxHealth)
    {
        if (health <= 0)
        {
            onDeath?.Invoke();
        }
    }

}

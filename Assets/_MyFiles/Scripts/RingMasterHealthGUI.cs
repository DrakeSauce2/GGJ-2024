using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingMasterHealthGUI : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] float visualChangeRate = 0.1f;
    int currentHealth, maxHealth;

    public delegate void OnHealthChanged(int health);
    public OnHealthChanged onHealthChanged;

    private bool valueChanging = false;

    private void Awake()
    {
        onHealthChanged += ChangeHealthValue;
        valueChanging = false;
    }

    public void Init(int health, int maxHealth)
    {
        currentHealth = health;
        this.maxHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    public void ChangeHealthValue(int health)
    {
        currentHealth = health;

        if (valueChanging == true)
            StartCoroutine(HealthSlideCoroutine());
    }

    private IEnumerator HealthSlideCoroutine()
    {
        valueChanging = true;

        while (healthSlider.value > currentHealth)
        {
            healthSlider.value -= Time.deltaTime;
            Debug.Log("Ringmaster Health GUI doing stuff!");

            yield return new WaitForEndOfFrame();
        }

        valueChanging = false;
    }

}

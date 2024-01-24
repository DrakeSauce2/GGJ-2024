using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGUI : MonoBehaviour
{
    public static HealthGUI Instance { get; private set; }   

    [SerializeField] List<GameObject> healthUnits = new List<GameObject>();

    private HealthComponent healthComponent;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void Init(HealthComponent owningHealthComponent)
    {
        healthComponent = owningHealthComponent;

        healthComponent.onHealthChanged -= UpdateGUI;
        healthComponent.onHealthChanged += UpdateGUI;
    }

    private void UpdateGUI(GameObject instigator, int health, int maxHealth)
    {
        for (int i = 0; i < maxHealth; i++)
        {
            if(i >= health)
                healthUnits[i].SetActive(false);
            else 
                healthUnits[i].SetActive(true);

        }
    }
}

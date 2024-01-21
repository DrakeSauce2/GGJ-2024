using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGUI : MonoBehaviour
{
    [SerializeField] List<GameObject> healthUnits = new List<GameObject>();

    private HealthComponent healthComponent;

    public void Init(HealthComponent owningHealthComponent)
    {
        healthComponent = owningHealthComponent;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    public GameObject Owner { get; private set; }
    private int damage;
    private int team;

    public void Init(GameObject owner, int damage, int team)
    {
        Owner = owner;
        this.damage = damage;
        this.team = team;
    }

    private void OnTriggerEnter(Collider other)
    {
        Character target = other.GetComponent<Character>();
        if (target == null) return;

        target.TakeDamage(Owner, damage, team);

    }
}

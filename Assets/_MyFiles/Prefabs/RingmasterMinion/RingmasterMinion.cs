using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingmasterMinion : Enemy
{
    public override void StartDeath()
    {
        base.StartDeath();

        isDead = true;

        RingMaster.Instance.RemoveMinionFromList(gameObject);

        Destroy(agent);

        ragdoll.EnableRagdoll();
        ragdoll.ApplyForce();

        gameObject.layer = 7;

        Destroy(gameObject, 3);
    }
}

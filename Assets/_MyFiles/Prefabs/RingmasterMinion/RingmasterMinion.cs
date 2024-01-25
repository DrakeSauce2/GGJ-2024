using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class RingmasterMinion : Enemy
{
    public override void StartDeath()
    {
        base.StartDeath();
        gameObject.layer = 7;

        isDead = true;

        RingMaster.Instance.RemoveMinionFromList(gameObject);

        //Destroy(agent);

        ragdoll.EnableRagdoll();
        ragdoll.ApplyForce();

        Destroy(gameObject, 3);
    }
}

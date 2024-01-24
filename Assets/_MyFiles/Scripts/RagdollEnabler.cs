using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollEnabler : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform ragdollRoot;
    [SerializeField] bool startRagdoll = false;

    [SerializeField] private Rigidbody[] rigidbodies;
    [SerializeField] private CharacterJoint[] joints;
    [SerializeField] private Collider[] colliders;

    [Header("Force")]
    [SerializeField] float ragdollForce;

    private void Awake()
    {
        rigidbodies = ragdollRoot.GetComponentsInChildren<Rigidbody>();
        joints = ragdollRoot.GetComponentsInChildren<CharacterJoint>();
        colliders = ragdollRoot.GetComponentsInChildren<Collider>();

        if (startRagdoll)
        {
            EnableRagdoll();
        }
        else
        {
            EnableAnimator();
        }
    }

    public void EnableRagdoll()
    {
        animator.enabled = false;
        foreach (CharacterJoint joint in joints)
        {
            joint.enableCollision = true;
        }
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
        }
    }

    public void ApplyForce()
    {
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.AddForce(-transform.forward * ragdollForce);
        }
        
    }

    public void EnableAnimator()
    {
        animator.enabled = true;
        foreach (CharacterJoint joint in joints)
        {
            joint.enableCollision = false;
        }
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
        }
    }
}

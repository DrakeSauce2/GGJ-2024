using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageProjectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 3f;

    Vector3 lastKnownPlayerPosition;

    private void Start()
    {
        lastKnownPlayerPosition = Player.Instance.transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, lastKnownPlayerPosition, projectileSpeed * Time.fixedDeltaTime);
    }

}

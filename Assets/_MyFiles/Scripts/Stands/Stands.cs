using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stands : MonoBehaviour
{

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] List<GarbageProjectile> throwables = new List<GarbageProjectile>();

    private void Awake()
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }

}

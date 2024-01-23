using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : MonoBehaviour
{
    private int xDir, yDir, zDir;
    private float xRate, yRate, zRate;

    private Vector3 currentRotation;

    private void Start()
    {
        xDir = Mathf.CeilToInt(Random.Range(-1, 1));
        yDir = Mathf.CeilToInt(Random.Range(-1, 1));
        zDir = Mathf.CeilToInt(Random.Range(-1, 1));

        xRate = Random.Range(0.5f, 5f) * xDir;
        yRate = Random.Range(0.5f, 5f) * yDir;
        zRate = Random.Range(0.5f, 5f) * zDir;

    }

    private void Update()
    {
        currentRotation = new Vector3(currentRotation.x + (xDir * Time.deltaTime), 
                                      currentRotation.y + (yDir * Time.deltaTime), 
                                      currentRotation.z + (zDir * Time.deltaTime));

        gameObject.transform.Rotate(currentRotation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GarbageType
{
    LINGER,
    IMPACT,
}

public class GarbageProjectile : MonoBehaviour
{
    [SerializeField] GarbageType type;
    [Space]
    [SerializeField] private float speed = 1f;
    [Space]
    [SerializeField] private GameObject trajectoryOutlinePrefab;
    private GameObject instancedTrajectoryOutline;

    [SerializeField] private float rangeFromPlayer = 3f;

    private Damager damager;

    Vector3 startingPosition;
    Vector3 targetPosition;

    [SerializeField] private bool doDamage = false;

    private void Start()
    {
        damager = GetComponent<Damager>();

        startingPosition = transform.position;
        targetPosition = GetRandomPosition(Player.Instance.transform.position, 3);

        Vector3 direction = targetPosition - startingPosition;
        Vector3 groundDirection = new Vector3(direction.x, 0, direction.z);
        Vector3 targetPos = new Vector3(groundDirection.magnitude, direction.y, 0);
        float height = targetPos.y + targetPos.magnitude / 2f;
        height = Mathf.Max(0.01f, height);
        float angle, velocity, time;

        CalculatePathWithHeight(targetPos, height, out velocity, out angle, out time);
        StartCoroutine(TrajectoryCoroutine(groundDirection.normalized, velocity, angle, time));

        if(trajectoryOutlinePrefab != null)
            instancedTrajectoryOutline = Instantiate(trajectoryOutlinePrefab, new Vector3(targetPosition.x, 0, targetPosition.z), trajectoryOutlinePrefab.transform.rotation);
        
    }

    private Vector3 GetRandomPosition(Vector3 startingPosition, float max)
    {
        return new Vector3(Random.Range(startingPosition.x - max, startingPosition.x + max),
                           startingPosition.y,
                           Random.Range(startingPosition.z - max, startingPosition.z + max));
    }

    private float QuadraticEquation(float a, float b, float c, float sin)
    {
        return (-b + sin * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
    }

    private void CalculatePathWithHeight(Vector3 targetPos, float height, out float velocity, out float angle, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float gravity = -Physics.gravity.y;

        float b = Mathf.Sqrt(2 * gravity * height);
        float a = (-0.5f * gravity);
        float c = -yt;

        float tmax = QuadraticEquation(a, b, c, 1);
        float tmin = QuadraticEquation(a, b, c, -1);
        time = tmax > tmin ? tmax : tmin;

        angle = Mathf.Atan(b * time / xt);
        velocity = b / Mathf.Sin(angle);

    }

    private IEnumerator TrajectoryCoroutine(Vector3 direction, float velocity, float angle, float time)
    {
        float t = 0;

        if(type == GarbageType.IMPACT)
        Destroy(gameObject, time / speed);

        while (t < time)
        {
            float x = velocity * t * Mathf.Cos(angle);
            float y = velocity * t * Mathf.Sin(angle) - (1f/2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            transform.position = startingPosition + direction * x + Vector3.up * y;

            t += (speed * Time.deltaTime);
            yield return null;
        }


        if (doDamage == true)
        {
            damager.StartDamage(0.5f);

            GetComponent<MeshFilter>().mesh = null;
        }
    }

    private void OnDestroy()
    {
        if(instancedTrajectoryOutline != null)
            Destroy(instancedTrajectoryOutline);
    }

}

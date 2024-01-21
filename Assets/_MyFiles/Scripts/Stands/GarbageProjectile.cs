using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GarbageType
{
    GOOD,
    BAD,
}

public class GarbageProjectile : MonoBehaviour
{
    [SerializeField] GarbageType type;

    private Damager damager;

    Vector3 startingPosition;
    Vector3 lastKnownPlayerPosition;

    private void Start()
    {
        damager = GetComponent<Damager>();

        startingPosition = transform.position;
        lastKnownPlayerPosition = Player.Instance.transform.position;

        Vector3 direction = lastKnownPlayerPosition - startingPosition;
        Vector3 groundDirection = new Vector3(direction.x, 0, direction.z);
        Vector3 targetPos = new Vector3(groundDirection.magnitude, direction.y, 0);
        float height = targetPos.y + targetPos.magnitude / 2f;
        height = Mathf.Max(0.01f, height);
        float angle, velocity, time;

        CalculatePathWithHeight(targetPos, height, out velocity, out angle, out time);
        StartCoroutine(TrajectoryCoroutine(groundDirection.normalized, velocity, angle, time));
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
        while (t < time)
        {
            float x = velocity * t * Mathf.Cos(angle);
            float y = velocity * t * Mathf.Sin(angle) - (1f/2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            transform.position = startingPosition + direction * x + Vector3.up * y;

            t += Time.deltaTime;
            yield return null;
        }

        if (type == GarbageType.BAD)
        {
            damager.StartDamage(0.5f);

            GetComponent<MeshFilter>().mesh = null;

            Destroy(gameObject, 1f);
        }

    }

}

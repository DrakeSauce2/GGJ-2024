using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DilateEffect : MonoBehaviour
{
    [SerializeField] float scaleSize = 1f;

    private void Awake()
    {
        StartCoroutine(Dilate());
    }

    private IEnumerator Dilate()
    {
        float t = 0.1f;
        float rate = scaleSize / 4f;
        while (t < scaleSize)
        {
            t += rate * Time.deltaTime;
            gameObject.transform.localScale = new Vector3 (t, t, t);

            yield return new WaitForEndOfFrame();
        }
    }

}

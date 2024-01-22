using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : MonoBehaviour
{
    [SerializeField] float duration = 3f;

    private void Awake()
    {
        StartCoroutine(AreaEffectDuration());
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player == null) return;

        TomatoOverlay.Instance.TomatoThem();
        Destroy(gameObject);
    }

    private IEnumerator AreaEffectDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }


}

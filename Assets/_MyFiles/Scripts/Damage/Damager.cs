using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] DamageTrigger trigger;
    [SerializeField] int damage;
    [SerializeField] int team;

    Coroutine damageCoroutine = null;
    
    private void Awake()
    {
        trigger.Init(gameObject, damage, team);
    }
    
    public void StartDamage(float triggerDuration)
    {
        if (damageCoroutine == null)
            damageCoroutine = StartCoroutine(DamageCoroutine(triggerDuration));
    }

    private IEnumerator DamageCoroutine(float triggerDuration)
    {
        trigger.gameObject.SetActive(true);

        yield return new WaitForSeconds(triggerDuration);

        trigger.gameObject.SetActive(false);
        damageCoroutine = null;
    }

}

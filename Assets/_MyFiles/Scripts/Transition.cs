using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    public static Transition Instance;

    [SerializeField] Image transitionImage;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

    }

    public IEnumerator CutIn()
    {
        float t = 1;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            transitionImage.fillAmount = t;
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator CutOut()
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transitionImage.fillAmount = t;
            yield return new WaitForEndOfFrame();
        }
    }



}

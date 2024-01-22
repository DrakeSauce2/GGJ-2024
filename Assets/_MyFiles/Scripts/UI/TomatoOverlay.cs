using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TomatoOverlay : MonoBehaviour
{
    public static TomatoOverlay Instance;

    [SerializeField] Image tomatoOverlay;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }

    public void TomatoThem()
    {
        tomatoOverlay.color = new Color(tomatoOverlay.color.r,
                                        tomatoOverlay.color.g, 
                                        tomatoOverlay.color.b,  
                                        tomatoOverlay.color.a + 0.3f);
    }

    private void Update()
    {
        if (tomatoOverlay.color.a > 0)
        {
            tomatoOverlay.color = new Color(tomatoOverlay.color.r,
                                        tomatoOverlay.color.g,
                                        tomatoOverlay.color.b,
                                        tomatoOverlay.color.a - (0.1f * Time.deltaTime));
        }
    }

}

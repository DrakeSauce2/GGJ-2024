using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TomatoOverlay : MonoBehaviour
{
    public static TomatoOverlay Instance;

    [SerializeField] Image tomatoOverlay;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hitClip;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }

    public void TomatoThem()
    {
        
        audioSource.clip = hitClip;
        audioSource.Play();

        tomatoOverlay.color = new Color(tomatoOverlay.color.r,
                                        tomatoOverlay.color.g, 
                                        tomatoOverlay.color.b,  
                                        1.5f);

    }

    private void Update()
    {
        audioSource.volume = AudioManager.Instance.SoundSettings.soundVolume;

        if (tomatoOverlay.color.a > 0)
        {
            tomatoOverlay.color = new Color(tomatoOverlay.color.r,
                                        tomatoOverlay.color.g,
                                        tomatoOverlay.color.b,
                                        tomatoOverlay.color.a - (0.3f * Time.deltaTime));
        }
    }

}

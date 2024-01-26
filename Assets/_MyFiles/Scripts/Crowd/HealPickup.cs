using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPickup : MonoBehaviour
{
    [SerializeField] private int healAmount;

    [Header("Audio")]
    [SerializeField] AudioSource _SFXSource;
    [SerializeField] private AudioClip pickupSound;

    private void Awake()
    {
        _SFXSource.volume = AudioManager.Instance.SoundSettings.soundVolume;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player)
        {
            AudioManager.Instance.PlaySoundEffect(pickupSound);

            player.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}

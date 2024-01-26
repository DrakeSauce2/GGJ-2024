using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sound Settings")]
    [SerializeField] private SoundSettings soundSettings;
    public SoundSettings SoundSettings { get { return soundSettings; } }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _SFXSource; // To Play Destroyed Objects SFX
    [SerializeField] private AudioSource _MusicSource;

    [Header("Volume Sliders")]
    [SerializeField] private Slider _SoundSlider;
    [SerializeField] private Slider _MusicSlider;

    [Header("Death Sounds")]
    [SerializeField] List<AudioClip> deathSounds;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);

        _SFXSource.volume = soundSettings.soundVolume;
        _SoundSlider.value = soundSettings.soundVolume;

        _MusicSlider.value = soundSettings.musicVolume;
        _MusicSource.volume = soundSettings.musicVolume;

    }

    private void Update()
    {
        SetSoundEffectVolume();

        SetMusicVolume();
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        if(audioClip == null) return;

        _SFXSource.clip = audioClip;
        _SFXSource.Play();
    }

    private void SetSoundEffectVolume()
    {

        soundSettings.soundVolume = _SFXSource.volume = _SoundSlider.value;
    }

    private void SetMusicVolume()
    {
        soundSettings.musicVolume = _MusicSource.volume = _MusicSlider.value;
    }

    public void SetMusic(AudioClip clip)
    {
        if (clip == null) return;

        _MusicSource.clip = clip;
        _MusicSource.Play();
    }

}

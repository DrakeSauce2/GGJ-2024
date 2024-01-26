using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SoundSettings")]
public class SoundSettings : ScriptableObject
{
    [Header("Music Volume")]
    [SerializeField, Range(0, 1)] float _MusicVolume;
    public float musicVolume 
    { 
        get { return _MusicVolume; }

        set
        {
            if (value < 0)
            {
                _MusicVolume = 0;
                return;
            }

            if (value > 1)
            {
                _MusicVolume = 1;
                return;
            }

            _MusicVolume = value;
        }

    }

    [Header("Sound Effect Volume")]
    [SerializeField, Range(0, 1)] float _SFXVolume;
    public float soundVolume 
    {
        get { return _SFXVolume; }

        set
        {
            if (value < 0)
            {
                _SFXVolume = 0;
                return;
            }

            if(value > 1)
            {
                _SFXVolume = 1;
                return;
            }

            _SFXVolume = value; 
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("FX Volume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("FX Volume", level);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("Music Volume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("Music Volume", level);
    }
}

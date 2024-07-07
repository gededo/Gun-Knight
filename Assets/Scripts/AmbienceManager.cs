using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AmbienceManager : MonoBehaviour
{
    public static AmbienceManager instance;

    [SerializeField] private AudioSource ambienceObject;
    [SerializeField] private AudioClip ambienceSound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        PlayAmbienceClip(ambienceSound, transform, 0.3f);
    }

    public void PlayAmbienceClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(ambienceObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioMixer musicMixer;

    [SerializeField] private AudioSource musicObject;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlayMusicCLip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(musicObject, spawnTransform.position, Quaternion.identity);

        StartCoroutine(StartFade(audioSource, 5f, volume));

        audioSource.clip = audioClip;

        audioSource.Play();

        DontDestroyOnLoad(audioSource);
    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}

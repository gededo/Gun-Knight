using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    [SerializeField] private AudioClip shotgunSound;

    public static DontDestroy instance;
    public bool hasShot = false;

    void Awake()
    {
        if (instance != null)
        {
            if (instance.hasShot)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SoundFXManager.instance.PlaySoundFXCLip(shotgunSound, transform, 0.1f);
        hasShot = true;
    }
}

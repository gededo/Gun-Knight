using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnStart : MonoBehaviour
{
    public static ResetOnStart Instance;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);
        Instance = this;

        PlayerPrefs.SetString("coins", "");
    }
}

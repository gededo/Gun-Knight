using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetOnStart : MonoBehaviour
{
    public static ResetOnStart Instance;
    public int sceneId;

    void Awake()
    {
        sceneId = SceneManager.GetActiveScene().buildIndex;

        if (Instance != null && Instance.sceneId == sceneId) 
        {
            Destroy(gameObject); 
            return; 
        }
        else if(Instance != null && Instance.sceneId != sceneId)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }

        GameObject music = GameObject.FindGameObjectWithTag("Music");
        if (music != null)
        {
            Destroy(music);
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;

        PlayerPrefs.SetString("coins", "");
        PlayerPrefs.SetString("equippedpowerups", "");
        PlayerPrefs.SetInt("wallet", 0);
    }
}

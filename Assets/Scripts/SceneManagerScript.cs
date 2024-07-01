using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene("fase3");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log(PlayerPrefs.GetString("coins"));
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            PlayerPrefs.SetString("coins", "");
        }
    }
}

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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //SceneManager.LoadScene("fase3");
            //PlayerPrefs.SetString("coins", "");
            //PlayerPrefs.SetInt("wallet", 0);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log(PlayerPrefs.GetString("coins"));
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            PlayerPrefs.SetString("coins", "");
            PlayerPrefs.SetInt("wallet", 0);
        }
    }
}

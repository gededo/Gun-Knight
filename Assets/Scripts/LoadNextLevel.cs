using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNextLevel : MonoBehaviour
{
    SceneManagerScript sceneManagerScript;

    private void Start()
    {
        sceneManagerScript = GameObject.Find("SceneManager").GetComponent<SceneManagerScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerPrefs.SetString("coins", "");
            PlayerPrefs.SetString("equippedpowerups", "");
            PlayerPrefs.SetInt("wallet", 0);
            sceneManagerScript.LoadNext();
        }
    }
}

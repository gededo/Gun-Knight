using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNextOnStart : MonoBehaviour
{
    SceneManagerScript sceneManagerScript;

    private void Start()
    {
        sceneManagerScript = GameObject.Find("SceneManager").GetComponent<SceneManagerScript>();
        sceneManagerScript.LoadNext();
    }
}

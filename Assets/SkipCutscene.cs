using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipCutscene : MonoBehaviour
{
    SceneManagerScript sceneManagerScript;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            sceneManagerScript = GameObject.Find("SceneManager").GetComponent<SceneManagerScript>();
            sceneManagerScript.LoadNext();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    SceneManagerScript sceneManagerScript;

    public float fadeInDuration = 0.4f;
    public CanvasGroup canvGroup;
    
    void Start()
    {
        canvGroup = GetComponent<CanvasGroup>();
        StartCoroutine(FadeIn(canvGroup));
        sceneManagerScript = GameObject.Find("SceneManager").GetComponent<SceneManagerScript>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sceneManagerScript.ReloadScene();
        }
    }

    public IEnumerator FadeIn(CanvasGroup canvGroup)
    {
        float counter = 0f;

        while(counter < fadeInDuration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(0, 1, counter / fadeInDuration);

            yield return null;
        }
    }
}

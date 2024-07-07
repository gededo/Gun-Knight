using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LoadNextLevel : MonoBehaviour
{
    SceneManagerScript sceneManagerScript;

    public float fadeInDuration = 0.4f;
    public CanvasGroup canvGroup;

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
            AudioSource currentSong = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
            MusicManager.instance.FadeOut(currentSong, 0.4f);
            StartCoroutine(FadeIn(canvGroup));
        }
    }

    public IEnumerator FadeIn(CanvasGroup canvGroup)
    {
        float counter = 0f;

        while (counter < fadeInDuration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(0, 1, counter / fadeInDuration);

            yield return null;
        }
        sceneManagerScript.LoadNext();
    }
}

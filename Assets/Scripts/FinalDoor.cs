using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    public bool inDoor = false;
    public float fadeInDuration = 0.6f;

    public CanvasGroup canvGroup;
    public Transform player;
    public GameObject prompt;

    SceneManagerScript sceneManagerScript;



    private void Awake()
    {
        sceneManagerScript = GameObject.Find("SceneManager").GetComponent<SceneManagerScript>();
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inDoor)
        {
            AudioSource currentSong = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
            MusicManager.instance.FadeOut(currentSong, 0.4f);
            StartCoroutine(FadeIn(canvGroup));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inDoor = true;
            prompt.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inDoor = false;
            prompt.SetActive(false);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeOnStart : MonoBehaviour
{
    public float fadeInDuration = 0.6f;
    public CanvasGroup canvGroup;

    void Start()
    {
        StartCoroutine(FadeIn(canvGroup));
    }

    public IEnumerator FadeIn(CanvasGroup canvGroup)
    {
        float counter = 0f;

        while (counter < fadeInDuration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(1, 0, counter / fadeInDuration);

            yield return null;
        }
    }
}

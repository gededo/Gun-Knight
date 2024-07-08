using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class FadeOnStart : MonoBehaviour
{
    public float fadeInDuration = 1f;
    public GameObject BlackSquare;

    void Start()
    {
        StartCoroutine(FadeIn(BlackSquare));
    }

    public IEnumerator FadeIn(GameObject target)
    {
        float counter = 0f;

        while (counter < fadeInDuration)
        {
            counter += Time.deltaTime;
            Color tmp = target.GetComponent<SpriteRenderer>().color;
            tmp.a = Mathf.Lerp(1, 0, counter / fadeInDuration);
            target.GetComponent<SpriteRenderer>().color = tmp;

            yield return null;
        }
    }
}

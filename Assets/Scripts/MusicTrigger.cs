using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public bool hasTriggered = false;

    [SerializeField] private AudioClip levelMusic;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !hasTriggered)
        {
            GameObject musicObject = GameObject.FindGameObjectWithTag("Music");
            if (musicObject == null)
            {
                MusicManager.instance.PlayMusicCLip(levelMusic, transform, 0.035f);
            }
            hasTriggered = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerScript = collision.GetComponent<PlayerController>();
            SoundFXManager.instance.PlaySoundFXCLip(playerScript.damageSoundClip, transform, 0.8f);
            playerScript.currentHealth = 0;
            playerScript.SetHealthSlider(playerScript.currentHealth);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poweruppickup : MonoBehaviour
{
    public PowerupEffect powerupEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            powerupEffect.Apply(collision.gameObject);
            Destroy(gameObject);
        }
    }
}

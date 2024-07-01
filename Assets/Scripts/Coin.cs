using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                playerScript.GetCoin(coinValue);
                PlayerPrefs.SetString("coins", (PlayerPrefs.GetString("coins") + gameObject.name));
                Destroy(gameObject);
            }
        }
    }
}
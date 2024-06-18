using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        { 
            PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
            playerScript.GetCoin(coinValue);
            Destroy(gameObject);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public PowerupEffect powerupEffect;

    public GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void ApplyEffect()
    {
        Debug.Log("Powerup Applied!");
        powerupEffect.Apply(player);
    }
}

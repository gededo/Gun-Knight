using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Guns
{
    public float rifleDamageMultiplier = 1f;

    void Awake()
    {
        GunDamage = rifleDamageMultiplier * 5f;
    }
}

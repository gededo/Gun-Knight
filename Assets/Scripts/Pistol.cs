using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Guns
{
    public float pistolDamageMultiplier = 10f;

    void Awake()
    {
        GunDamage = pistolDamageMultiplier * 2f;
    }
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Guns
{
    public float pistolDamageMultiplier = 1f;

    void Awake()
    {
        GunDamage = pistolDamageMultiplier * 3f;
    }
}

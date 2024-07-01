using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/DamageBoost")]
public class DamageBoost : PowerupEffect
{
    public float amount;

    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerController>().damageMultiplier = amount;
    }
}

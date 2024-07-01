using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/ShieldPowerup")]
public class ShieldPUp : PowerupEffect
{
    public float amount;

    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerController>().maxShields = amount;
        target.GetComponent<PlayerController>().shields = amount;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/SpeedBoost")]
public class SpeedBoost : PowerupEffect
{
    public float amount;

    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerController>().speed = amount;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/HealthBoost")]
public class HealthBoost : PowerupEffect
{
    public float amount;

    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerController>().maxHealth = amount;
        target.GetComponent<PlayerController>().currentHealth = amount;
        target.GetComponent<PlayerController>().healthSlider.maxValue = amount;
        target.GetComponent<PlayerController>().SetHealthSlider(amount);
    }
}

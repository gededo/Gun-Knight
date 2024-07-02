using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/GunPowerup")]
public class GunPUp : PowerupEffect
{
    public string gunName;

    public override void Apply(GameObject target)
    {
        if(gunName == "Rifle")
        {
            target.GetComponent<PlayerController>().boughtRifle = true;
        }
        else if (gunName == "Shotgun")
        {
            target.GetComponent<PlayerController>().boughtShotgun = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldHud : MonoBehaviour
{
    public int currentShieldAmount;

    public Image[] shields;
    public Sprite shieldSprite;

    public void UpdateShieldDisplay(int numOfShields)
    {
        for (int i = 0; i < shields.Length; i++)
        {
            if (i < numOfShields)
            {
                shields[i].enabled = true;
            }
            else
            {
                shields[i].enabled = false;
            }
        }
    }
}

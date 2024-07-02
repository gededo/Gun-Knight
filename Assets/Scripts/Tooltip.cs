using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    public GameObject player;
    public string message;
    public bool hasBought;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnMouseEnter()
    {
        TooltipManager._instance.powerupEffect = powerupEffect;
        TooltipManager._instance.selected = this;
        TooltipManager._instance.SetAndShowToolTip(message);
        TooltipManager._instance.player = player;
        TooltipManager._instance.hoveringOver = true;
    }

    private void OnMouseExit()
    {
        TooltipManager._instance.HideTooltip();
        TooltipManager._instance.powerupEffect = null;
        TooltipManager._instance.selected = null;
        TooltipManager._instance.player = null;
        TooltipManager._instance.hoveringOver = false;
    }
}

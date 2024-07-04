using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    public GameObject player;
    public GameObject arrows;
    public GameObject rifleTooltip;
    public GameObject shotgunTooltip;
    public string message;
    public bool hasBought;
    public bool isRifle;
    public bool a;

    private void Start()
    {
        a = PlayerPrefs.GetString("equippedpowerups").Contains(gameObject.transform.parent.gameObject.name);
        if (rifleTooltip != null && shotgunTooltip != null)
        {
            if (gameObject.transform.parent.name == rifleTooltip.name)
            {
                isRifle = true;
                if (a)
                {
                    shotgunTooltip.SetActive(true);
                }
            }
            else if (gameObject.transform.parent.name == shotgunTooltip.name)
            {
                bool b = PlayerPrefs.GetString("equippedpowerups").Contains("Rifle PUp");
                isRifle = false;
                if (a)
                {
                    shotgunTooltip.SetActive(false);
                    rifleTooltip.SetActive(true);
                }
            }
        }
        player = GameObject.Find("Player");
    }

    private void OnMouseEnter()
    {
        if (arrows != null) 
        { 
            arrows.SetActive(true);
        }
        TooltipManager._instance.powerupEffect = powerupEffect;
        TooltipManager._instance.selected = this;
        TooltipManager._instance.SetAndShowToolTip(message);
        TooltipManager._instance.player = player;
        TooltipManager._instance.hoveringOver = true;
    }

    private void OnMouseExit()
    {
        if (arrows != null)
        {
            arrows.SetActive(false);
        }
        TooltipManager._instance.HideTooltip();
        TooltipManager._instance.powerupEffect = null;
        TooltipManager._instance.selected = null;
        TooltipManager._instance.player = null;
        TooltipManager._instance.hoveringOver = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopSwitchArrows : MonoBehaviour
{
    public GameObject parentTooltip;
    public GameObject rifleButton;
    public GameObject shotgunButton;
    
    public void SwitchArrowsLeft()
    {
        if (!parentTooltip.GetComponent<Tooltip>().isRifle)
        {
            Tooltip rifleTooltip = rifleButton.GetComponentInChildren<Tooltip>();
            rifleTooltip.a = PlayerPrefs.GetString("equippedpowerups").Contains(rifleButton.name);
            if (!rifleTooltip.a)
            {
                rifleButton.SetActive(true);
                shotgunButton.SetActive(false);
            }
        }
    }

    public void SwitchArrowsRight()
    {
        if (parentTooltip.GetComponent<Tooltip>().isRifle)
        {
            Tooltip shotgunTooltip = shotgunButton.GetComponentInChildren<Tooltip>();
            shotgunTooltip.a = PlayerPrefs.GetString("equippedpowerups").Contains(shotgunButton.name);
            if (!shotgunTooltip.a)
            {
                rifleButton.SetActive(false);
                shotgunButton.SetActive(true);
            }
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject shotgunPUp;
    public GameObject riflePUp;
    bool d = false;

    void Start()
    {
        bool a = PlayerPrefs.GetString("coins").Contains(gameObject.name);
        bool b = PlayerPrefs.GetString("equippedpowerups").Contains(gameObject.name);

        if (gameObject.name == "Rifle PUp")
        {
            d = PlayerPrefs.GetString("equippedpowerups").Contains(shotgunPUp.transform.parent.gameObject.name);
        }

        if (a)
        {
            gameObject.SetActive(false);
        }
        else if (b)
        {
            GameObject player = GameObject.Find("Player");
            Tooltip childPUPScript = GetComponentInChildren<Tooltip>();
            TooltipManager._instance.powerupEffect = childPUPScript.powerupEffect;
            TooltipManager._instance.player = player;
            TooltipManager._instance.ApplyEffect();
            TooltipManager._instance.powerupEffect = null;
            TooltipManager._instance.player = null;
            gameObject.SetActive(false);
        }
        if (gameObject.name == "Rifle PUp" && d && (shotgunPUp != null))
        {
            GameObject player = GameObject.Find("Player");
            Tooltip shotgunPUpScript = shotgunPUp.GetComponent<Tooltip>();
            TooltipManager._instance.powerupEffect = shotgunPUpScript.powerupEffect;
            TooltipManager._instance.player = player;
            TooltipManager._instance.ApplyEffect();
            TooltipManager._instance.powerupEffect = null;
            TooltipManager._instance.player = null;
        }
    }
}

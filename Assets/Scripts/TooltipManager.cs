using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEditor.Timeline.Actions;
using Unity.VisualScripting;

public class TooltipManager : MonoBehaviour
{
    public bool hoveringOver = false;
    public bool isNear;

    public PowerupEffect powerupEffect;
    public GameObject player;
    public PlayerController playerScript;
    public Tooltip selected;
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI textComponent2;

    public static TooltipManager _instance;

    [SerializeField] private AudioClip cashSoundClip;
    [SerializeField] private AudioClip errorSoundClip;

    private void Awake()
    {
        /*if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {*/
            _instance = this;
        //}
    }

    void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && hoveringOver && isNear && !selected.hasBought)
        {
            if (playerScript.coinScore >= powerupEffect.price)
            {
                ApplyEffect();
                selected.hasBought = true;
                PlayerPrefs.SetString("equippedpowerups", (PlayerPrefs.GetString("equippedpowerups") + selected.gameObject.transform.parent.gameObject.name));
                selected.gameObject.transform.parent.gameObject.SetActive(false);
                HideTooltip();
                playerScript.coinScore -= powerupEffect.price;
                PlayerPrefs.SetInt("wallet", PlayerPrefs.GetInt("wallet") - powerupEffect.price);
                if (selected.rifleTooltip != null && selected.shotgunTooltip != null)
                {
                    if (selected.isRifle && !selected.shotgunTooltip.GetComponentInChildren<Tooltip>().hasBought)
                    {
                        selected.shotgunTooltip.
                            GetComponentInChildren<Tooltip>().a = PlayerPrefs.GetString("equippedpowerups").Contains(selected.shotgunTooltip.gameObject.name);
                        if (!selected.shotgunTooltip.GetComponentInChildren<Tooltip>().a)
                        {
                            selected.shotgunTooltip.SetActive(true);
                        }
                    }
                    else if (!selected.isRifle && !selected.rifleTooltip.GetComponentInChildren<Tooltip>().hasBought)
                    {
                        bool rifleBought = PlayerPrefs.GetString("equippedpowerups").Contains("Rifle PUp");
                        if (!rifleBought)
                        {
                            selected.rifleTooltip.SetActive(true);
                        }
                    }
                }
                SoundFXManager.instance.PlaySoundFXCLip(cashSoundClip, transform, 0.8f);
                powerupEffect = null;
                selected = null;
                player = null;
                hoveringOver = false;
                playerScript.UpdateScoreText();
            }
            else
            {
                SoundFXManager.instance.PlaySoundFXCLip(errorSoundClip, transform, 0.7f);
            }
        }
    }

    public void SetAndShowToolTip(string message)
    {
        if (isNear)
        {
            gameObject.SetActive(true);
            textComponent.text = message;
            textComponent2.text = "Press E to buy: " + powerupEffect.price.ToString();
        }
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
        textComponent.text = string.Empty;
        textComponent2.text = string.Empty;
    }

    public void ApplyEffect()
    {
        if (powerupEffect != null && player != null)
        {
            //Debug.Log("Powerup Applied!");
            powerupEffect.Apply(player);
        }
    }
}

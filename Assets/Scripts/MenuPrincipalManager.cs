using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipalManager : MonoBehaviour
{
    [SerializeField] private string Leveldojogo;
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelopcoes;
    [SerializeField] private AudioClip selectSoundClip;
    [SerializeField] private Slider fxVolSlider;
    [SerializeField] private Slider musicVolSlider;

    private void Start()
    {
        if (PlayerPrefs.GetFloat("FX Volume") != 0)
        {
            fxVolSlider.value = PlayerPrefs.GetFloat("FX Volume");
        }
        if (PlayerPrefs.GetFloat("Music Volume") != 0)
        {
            musicVolSlider.value = PlayerPrefs.GetFloat("Music Volume");
        }
    }

    public void Play() {
        PlayerPrefs.SetString("coins", "");
        PlayerPrefs.SetString("equippedpowerups", "");
        PlayerPrefs.SetInt("wallet", 0);
        SceneManager.LoadScene(Leveldojogo);
   }

   public void Optionsmenu() {
     painelMenuInicial.SetActive(false);
     painelopcoes.SetActive(true);
   }

   public void Closeoptionsmenu() {
     painelMenuInicial.SetActive(true);
     painelopcoes.SetActive(false);
   }

   public void Closethegame() {
    Debug.Log("Saiu do Jogo");
     Application.Quit();
   }

    public void SelectSound()
    {
        SoundFXManager.instance.PlaySoundFXCLip(selectSoundClip, transform, 0.1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipalManager : MonoBehaviour
{
    public float fadeInDuration = 0.6f;
    public CanvasGroup canvGroup;

    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelopcoes;
    [SerializeField] private AudioClip selectSoundClip;
    [SerializeField] private Slider fxVolSlider;
    [SerializeField] private Slider musicVolSlider;
    [SerializeField] private AudioClip menuMusic;

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
        MusicManager.instance.PlayMusicCLip(menuMusic, transform, 0.08f);
    }

    public void Play() {
        AudioSource currentSong = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        MusicManager.instance.FadeOut(currentSong, 0.4f);
        PlayerPrefs.SetString("coins", "");
        PlayerPrefs.SetString("equippedpowerups", "");
        PlayerPrefs.SetInt("wallet", 0);
        SoundFXManager.instance.PlaySoundFXCLip(selectSoundClip, transform, 0.1f);
        StartCoroutine(FadeOut(canvGroup));
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
        SoundFXManager.instance.PlaySoundFXCLip(selectSoundClip, transform, 0.1f);
        Application.Quit();
   }

    public void SelectSound()
    {
        SoundFXManager.instance.PlaySoundFXCLip(selectSoundClip, transform, 0.1f);
    }

    public IEnumerator FadeOut(CanvasGroup canvGroup)
    {
        float counter = 0f;

        while (counter < fadeInDuration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(0, 1, counter / fadeInDuration);

            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

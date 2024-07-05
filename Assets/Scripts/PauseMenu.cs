using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    [SerializeField] private GameObject painelopcoes;
    [SerializeField] private GameObject mainPausePanel;
    [SerializeField] private AudioClip selectSoundClip;
    [SerializeField] private Slider fxVolSlider;
    [SerializeField] private Slider musicVolSlider;

    private bool isPaused = false;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
            SoundFXManager.instance.PlaySoundFXCLip(selectSoundClip, transform, 0.1f);
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        mainPausePanel.SetActive(true);
        painelopcoes.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Optionsmenu()
    {
        painelopcoes.SetActive(true);
        mainPausePanel.SetActive(false);
    }

    public void Closeoptionsmenu()
    {
        mainPausePanel.SetActive(true);
        painelopcoes.SetActive(false);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void SelectSound()
    {
        SoundFXManager.instance.PlaySoundFXCLip(selectSoundClip, transform, 0.1f);
    }
}

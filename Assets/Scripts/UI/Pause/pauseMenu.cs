using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pauseMenu : MenuClass
{
    [SerializeField]
    private Slider sfxSlider, musicSlider;
    // Start is called before the first frame update
    void Start()
    {
        sfxSlider.value = AudioManager.Instance.soundVolume;
        musicSlider.value = AudioManager.Instance.musicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.Instance.player.GetComponent<Player>().state == PlayerState.Active ||
            UIManager.Instance.player.GetComponent<Player>().state == PlayerState.InMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleMenu();
            }
        }
    }

    public void SetSFX()
    {
        AudioManager.Instance.soundVolume = sfxSlider.value;
        AudioManager.Instance.SetSoundVolume(AudioManager.Instance.soundVolume);
    }

    public void SetMusic()
    {
        AudioManager.Instance.musicVolume = musicSlider.value;
        AudioManager.Instance.SetMusicVolume(AudioManager.Instance.musicVolume);
    }

    public void returnToGame()
    {
        Time.timeScale = 1;
        ToggleMenu();
    }

    public void quitToMainMenu()
    {
        ToggleMenu();
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public void saveGame()
    {
        EventManager.OnSaveEvent();
    }
}

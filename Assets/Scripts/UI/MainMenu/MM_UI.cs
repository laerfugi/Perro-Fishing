using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MM_UI : MonoBehaviour
{
    public string mainScene;
    [SerializeField]
    private GameObject mainScreen, settingsScreen, sfxSlider, musicSlider;
    // Start is called before the first frame update
    void Start()
    {
        sfxSlider.GetComponent<Slider>().value = AudioManager.Instance.soundVolume;
        musicSlider.GetComponent<Slider>().value = AudioManager.Instance.musicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(mainScene, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void setSFX()
    {
        AudioManager.Instance.soundVolume = sfxSlider.GetComponent<Slider>().value;
        AudioManager.Instance.SetSoundVolume(AudioManager.Instance.soundVolume);
    }

    public void setMusic()
    {
        AudioManager.Instance.musicVolume = musicSlider.GetComponent<Slider>().value;
        AudioManager.Instance.SetMusicVolume(AudioManager.Instance.musicVolume);
    }

    public void openSettings()
    {
        settingsScreen.SetActive(true);
        mainScreen.SetActive(false);
    }

    public void closeSettings()
    {
        mainScreen.SetActive(true);
        settingsScreen.SetActive(false);
    }
}

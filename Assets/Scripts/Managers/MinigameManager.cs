using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//starts/stops minigame scenes
public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }
    private EventSystem eventSystem;
    private string minigameSceneName;

    public bool hasWon;

    private void Awake()
    {
        Instance = this;

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        eventSystem = GameObject.FindObjectOfType<EventSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMinigame(string name)
    {
        minigameSceneName = "Minigame";
        SceneManager.LoadScene("Minigame", LoadSceneMode.Additive);
        if (eventSystem != null) eventSystem.enabled = false;

        EventManager.OnOpenMenuEvent();
        EventManager.OnStartMinigameEvent();
    }

    public void EndMinigame(bool result)
    {
        hasWon = result;
        SceneManager.UnloadSceneAsync(minigameSceneName);
        if (eventSystem != null) eventSystem.enabled = true;

        EventManager.OnCloseMenuEvent();
        EventManager.OnEndMinigameEvent(hasWon);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//starts/ends minigame scenes
public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }
    private EventSystem eventSystem;
    private string minigameSceneName;

    public MinigameTransition minigameTransition;   //coupled ui stuff

    //public bool startGame;

    public Minigame currentMinigame;

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
        //StartCoroutine(StartMinigame(null));
        StartCoroutine(StartMinigames(2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*---Private methods---*/
    #region Private methods
    private IEnumerator StartMinigameCoroutine(string name)
    {
        //reset vars
        minigameSceneName = "Minigame";

        //disable event system
        if (eventSystem != null) eventSystem.enabled = false;

        //transition and load scene
        SceneManager.LoadScene("Minigame", LoadSceneMode.Additive);
        yield return minigameTransition.StartCoroutine(minigameTransition.OpenCurtains());
        
        //start minigame

        //wait until game is done
        yield return new WaitUntil(() => currentMinigame.minigameState == MinigameState.Finish);

        //end game
        yield return minigameTransition.StartCoroutine(minigameTransition.CloseCurtains());
        SceneManager.UnloadSceneAsync(minigameSceneName);
    }

    private IEnumerator EndMinigameCoroutine()
    {
        //Transition
        yield return minigameTransition.StartCoroutine(minigameTransition.OpenCurtains());

        //reenable event system
        if (eventSystem != null) eventSystem.enabled = true;

        //Event call
        EventManager.OnCloseMenuEvent();
    }
    #endregion

    /*---public methods to start minigame(s)---*/
    #region public methods
    public IEnumerator StartMinigame(string name)
    {
        //Event call
        EventManager.OnOpenMenuEvent();

        yield return minigameTransition.StartCoroutine(minigameTransition.CloseCurtains());
        yield return StartMinigameCoroutine(name);
        yield return EndMinigameCoroutine();
    }

    public IEnumerator StartMinigames(int count)
    {
        //Event call
        EventManager.OnOpenMenuEvent();

        yield return minigameTransition.StartCoroutine(minigameTransition.CloseCurtains());
        for (int i = 0; i < count; i++)
        {
            Debug.Log("minigame " + i);
            yield return StartMinigameCoroutine(null);
            yield return new WaitUntil(()=> currentMinigame.minigameState == MinigameState.Finish);
        }

        yield return EndMinigameCoroutine();
    }
    #endregion
}

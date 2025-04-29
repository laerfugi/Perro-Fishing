using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//Launches/Closes minigame scenes
public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }
    private EventSystem eventSystem;
    private string minigameSceneName;

    [Header("Minigame Transition")]
    public MinigameTransition minigameTransition;   //coupled ui stuff

    [Header("Current Minigame")]
    public Minigame currentMinigame;

    public List<Result> results;

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
        //StartCoroutine(LaunchMinigames(2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*---Private methods---*/
    #region Private methods
    private IEnumerator LaunchMinigameCoroutine(string name)
    {
        //reset vars
        minigameSceneName = "Minigame";

        //disable event system
        if (eventSystem != null) eventSystem.enabled = false;

        //transition and load scene
        SceneManager.LoadScene("Minigame", LoadSceneMode.Additive);
        yield return minigameTransition.StartCoroutine(minigameTransition.OpenCurtains());

        //start the minigame with an event call
        EventManager.OnStartMinigameEvent();

        //wait until game is done
        yield return new WaitUntil(() => currentMinigame.minigameState == MinigameState.Finish);

        //update results list
        //results.Add(currentMinigame.result);
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i] == Result.Null) { results[i] = currentMinigame.result; break; }
        }


        //end game
        yield return minigameTransition.StartCoroutine(minigameTransition.CloseCurtains());
        SceneManager.UnloadSceneAsync(minigameSceneName);
    }

    private IEnumerator CloseMinigameCoroutine()
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
    public IEnumerator LaunchMinigame(string name)
    {
        //Event call
        EventManager.OnOpenMenuEvent();

        //reset results list
        results.Clear();
        for (int i = 0; i < 1; i++)
        {
            results.Add(Result.Null);
        }

        yield return minigameTransition.StartCoroutine(minigameTransition.CloseCurtains());
        yield return LaunchMinigameCoroutine(name);
        yield return CloseMinigameCoroutine();
    }

    public IEnumerator LaunchMinigames(int count)
    {
        //Event call
        EventManager.OnOpenMenuEvent();

        //reset results list
        results.Clear();
        for (int i = 0; i < count; i++)
        {
            results.Add(Result.Null);
        }

            yield return minigameTransition.StartCoroutine(minigameTransition.CloseCurtains());
        for (int i = 0; i < count; i++)
        {
            Debug.Log("minigame " + i);
            yield return LaunchMinigameCoroutine(null);
            yield return new WaitUntil(()=> currentMinigame.minigameState == MinigameState.Finish);

            if (results.Contains(Result.Lose)) { break; }
        }

        yield return CloseMinigameCoroutine();
    }
    #endregion
}

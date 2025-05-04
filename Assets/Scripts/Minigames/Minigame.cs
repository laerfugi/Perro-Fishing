using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//Singleton to be added in each minigame scene. Controls flow of the minigame and a bool to show if player has won.
public enum MinigameState {Start, Play, End, Finish}
public enum Result {Null, Win, Lose}

public class Minigame : MonoBehaviour
{
    private EventSystem eventSystem;

    public static Minigame Instance;

    [Header("Minigame Data")]
    public MinigameData minigameData;
    private string startMessage;
    private string controlsMessage;
    private float maxMinigameTime;

    [Header("Minigame Contents")]
    public GameObject contents;

    [Header("DEBUG")]
    //[Header("States")]
    public MinigameState minigameState;
    public Result result;
    private bool startMinigame;

    //[Header("Time")]
    public float minigameTime;
    public float startTime;
    public float endTime;

    private void OnEnable()
    {
        EventManager.StartMinigameEvent += ()=> startMinigame = true;
    }
    private void OnDisable()
    {
        EventManager.StartMinigameEvent -= () => startMinigame = true;
    }

    private void Awake()
    {
        Instance = this;

        if (EventSystem.current != null) eventSystem = EventSystem.current;

        result = Result.Lose;
        startMinigame = false;
        startMessage = minigameData.startMessage;
        controlsMessage = "Use " + minigameData.minigameControls.ToString();
        maxMinigameTime = minigameData.maxMinigameTime;

        if (MinigameManager.Instance != null) { MinigameManager.Instance.currentMinigame = this; }
        if (MinigameManager.Instance != null) { contents.transform.position = new Vector3(100, 100, 0); }       //offset contents if the scene's being launched by MinigameManager
}

    IEnumerator Start()
    {
        if (MinigameManager.Instance != null) { yield return new WaitUntil(() => startMinigame == true); }
        yield return StartCoroutine(StartMinigame());
        yield return StartCoroutine(PlayMinigame());
        yield return StartCoroutine(EndMinigame());
    }

    /*---States---*/
    #region States
    IEnumerator StartMinigame()
    {
        //reset vars and display message
        minigameState = MinigameState.Start;

        result = Result.Lose;
        minigameTime = maxMinigameTime;

        //event system
        if (eventSystem!= null) eventSystem.enabled = false;

        //show start messages
        MinigameUI.Instance.startMessage.text = startMessage;
        MinigameUI.Instance.startMessage.gameObject.SetActive(true);
        MinigameUI.Instance.controlsMessage.text = controlsMessage;
        MinigameUI.Instance.controlsMessage.gameObject.SetActive(true);

        yield return new WaitForSeconds(startTime);

        MinigameUI.Instance.startMessage.gameObject.SetActive(false);
        MinigameUI.Instance.controlsMessage.gameObject.SetActive(false);

        //event system
        if (eventSystem != null) eventSystem.enabled = true;
    }

    IEnumerator PlayMinigame()
    {
        //change state
        minigameState = MinigameState.Play;

        minigameTime = maxMinigameTime;
        float temp = maxMinigameTime;
        while (minigameTime > 0)
        {
            //call tick event
            if (temp != Mathf.Round(minigameTime))
            {
                temp = Mathf.Round(minigameTime);
                EventManager.OnTick();
            }

            minigameTime -= Time.deltaTime;
            minigameTime = Mathf.Clamp(minigameTime, 0.0f, maxMinigameTime);

            yield return null;
        }
    }

    IEnumerator EndMinigame()
    {
        //if minigame ends earlier than default call, don't run default call
        if (minigameState == MinigameState.End) { yield break; }

        //change state
        minigameState = MinigameState.End;

        //event system
        if (eventSystem != null) eventSystem.enabled = false;

        //when timer ends, show win/lose message
        if (result == Result.Win) { MinigameUI.Instance.ShowWinMessage(); }
        else if (result == Result.Lose) { MinigameUI.Instance.ShowLoseMessage(); }

        yield return new WaitForSeconds(endTime);

        //change state
        minigameState = MinigameState.Finish;
    }

    #endregion

    /*---Methods to control the game---*/
    
    /// <summary>
    /// At minigame start, result = Lose. Use these methods to change the state. 
    /// </summary>
    
    #region Methods to control the game

    //passively sets result
    public void Win()
    {
        result = Result.Win;
    }

    public void Lose()
    {
        result = Result.Lose;
    }

    //instantly sets result and ends game.
    public void InstantWin()
    {
        result = Result.Win;
        MinigameUI.Instance.ShowWinMessage();

        StartCoroutine(EndMinigame());
    }

    public void InstantLose()
    {
        result = Result.Lose;
        MinigameUI.Instance.ShowLoseMessage();

        StartCoroutine(EndMinigame());
    }

    #endregion
}

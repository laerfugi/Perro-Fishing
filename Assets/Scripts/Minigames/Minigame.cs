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
    public EventSystem eventSystem;

    public static Minigame Instance;

    [Header("Minigame Info")]
    public string startMessage;
    public float maxMinigameTime;

    [Header("States")]
    public MinigameState minigameState;
    public Result result;
    private bool startMinigame;

    [Header("Time")]
    public float startTime;
    public float minigameTime;
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
        result = Result.Lose;
        startMinigame = false;
    }

    IEnumerator Start()
    {
        if (MinigameManager.Instance != null) { MinigameManager.Instance.currentMinigame = this; }
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
        eventSystem.enabled = false;

        //
        MinigameUI.Instance.startMessage.text = startMessage;
        MinigameUI.Instance.startMessage.gameObject.SetActive(true);

        yield return new WaitForSeconds(startTime);

        MinigameUI.Instance.startMessage.gameObject.SetActive(false);

        //event system
        eventSystem.enabled = true;
    }

    IEnumerator PlayMinigame()
    {
        //change state
        minigameState = MinigameState.Play;

        minigameTime = maxMinigameTime;

        while (minigameTime > 0)
        {
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
        eventSystem.enabled = false;

        //when timer ends, show win/lose message
        if (result == Result.Win) { MinigameUI.Instance.ShowWinMessage(); }
        else if (result == Result .Lose) { MinigameUI.Instance.ShowLoseMessage(); }

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

    public void InstantLose()   //don't recommend to use this bc warioware doesnt use it
    {
        if (result != Result.Win)   //can't lose after you win
        {
            result = Result.Lose;
            MinigameUI.Instance.ShowLoseMessage();

            StartCoroutine(EndMinigame());
        }
    }

    #endregion
}

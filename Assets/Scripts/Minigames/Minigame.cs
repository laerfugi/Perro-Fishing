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

public class Minigame : MonoBehaviour
{
    public EventSystem eventSystem;

    public static Minigame Instance;

    [Header("States")]
    public MinigameState minigameState;
    public bool hasWon;

    [Header("Time")]
    public float startTime;
    public float minigameTime;
    public float maxMinigameTime;
    public float endTime;

    private void Awake()
    {
        hasWon = false;
        Instance = this;
    }

    IEnumerator Start()
    {
        //if (MinigameManager.Instance != null) { yield return new WaitUntil(() => MinigameManager.Instance.startGame); }
        if (MinigameManager.Instance != null) { MinigameManager.Instance.currentMinigame = this; }
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

        hasWon = false;
        minigameTime = maxMinigameTime;

        //event system
        eventSystem.enabled = false;

        //ui
        MinigameUI.Instance.startMessage.SetActive(true);

        yield return new WaitForSeconds(startTime);

        MinigameUI.Instance.startMessage.SetActive(false);

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
        if (hasWon) { MinigameUI.Instance.ShowWinMessage(); }
        else if (!hasWon) { MinigameUI.Instance.ShowLoseMessage(); }

        yield return new WaitForSeconds(endTime);

        //change state
        minigameState = MinigameState.Finish;
    }

    #endregion

    public void Win()
    {
        hasWon = true;
        MinigameUI.Instance.ShowWinMessage();

        StartCoroutine(EndMinigame());
    }

    public void Lose()
    {
        hasWon = false;
        MinigameUI.Instance.ShowLoseMessage();

        StartCoroutine(EndMinigame());
    }
}

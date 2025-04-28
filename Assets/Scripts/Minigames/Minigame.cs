using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//Singleton to be added in each minigame scene. Controls flow of the minigame and a bool to show if player has won.
public enum MinigameState {Start, Play, End}

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
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(StartMinigame());
    }

    private void Update()
    {
        if (minigameState == MinigameState.Play)
        {
            CheckTime();
        }
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
        
        //change state
        minigameState = MinigameState.Play;

        MinigameUI.Instance.startMessage.SetActive(false);

        //event system
        eventSystem.enabled = true;
    }
    private void CheckTime()
    {
        minigameTime -= Time.deltaTime;
        minigameTime = Mathf.Clamp(minigameTime, 0.0f, maxMinigameTime);

        //ui
        MinigameUI.Instance.timeText.text = "time: " + minigameTime.ToString();

        //change state
        if (minigameTime <= 0.0f)
        {
            StartCoroutine(EndMinigame());
        }
    }

    IEnumerator EndMinigame()
    {
        minigameState = MinigameState.End;

        //event system
        eventSystem.enabled = false;

        //ui
        if (hasWon) { MinigameUI.Instance.ShowWinMessage(); }
        else if (!hasWon) { MinigameUI.Instance.ShowLoseMessage(); }

        yield return new WaitForSeconds(endTime);

        //change state
        if (MinigameManager.Instance != null) MinigameManager.Instance.EndMinigame(hasWon);
    }

    #endregion

    public void Win()
    {
        hasWon = true;
    }

    public void Lose()
    {
        hasWon = false;
    }
}

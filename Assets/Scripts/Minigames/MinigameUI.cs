using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

//Singleton contains Minigame UI methods
public class MinigameUI : MonoBehaviour
{
    public static MinigameUI Instance;

    [Header("Start Message")]
    public GameObject startMessage;

    [Header("Time")]
    public TMP_Text timeText;

    [Header("End Messages")]
    public GameObject winMessage;
    public GameObject loseMessage;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        startMessage.SetActive(false);
        winMessage.SetActive(false); loseMessage.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        UpdateClock();
    }

    private void UpdateClock()
    {
        timeText.text = Mathf.Round(Minigame.Instance.minigameTime).ToString();

        if (Minigame.Instance.minigameTime <= 0 && !Minigame.Instance.hasWon) { timeText.text = ":("; }
    }

    /*---public methods---*/
    #region public methods
    public void ShowWinMessage()
    {
        loseMessage.SetActive(false);
        winMessage.SetActive(true);
    }

    public void ShowLoseMessage()
    {
        winMessage.SetActive(false);
        loseMessage.SetActive(true);
    }
    #endregion
}

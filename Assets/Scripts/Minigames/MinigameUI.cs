using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

//Singleton contains Minigame UI methods
public class MinigameUI : MonoBehaviour
{
    public static MinigameUI Instance;

    [Header("Start Messages")]
    public TMP_Text startMessage;
    public TMP_Text controlsMessage;

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
        startMessage.gameObject.SetActive(false); controlsMessage.gameObject.SetActive(false); 
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

        if (Minigame.Instance.minigameTime <= 0 && Minigame.Instance.result == Result.Lose) { timeText.text = ":("; }
    }

    /*---public methods---*/
    #region public methods
    public void ShowWinMessage()
    {
        loseMessage.SetActive(false);
        winMessage.SetActive(true);
        AudioManager.Instance.PlaySound("MinigameWin");
    }

    public void ShowLoseMessage()
    {
        winMessage.SetActive(false);
        loseMessage.SetActive(true);
        AudioManager.Instance.PlaySound("MinigameFail");
    }
    #endregion
}

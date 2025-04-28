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
        
    }



    public void ShowWinMessage()
    {
        winMessage.SetActive(true);
    }

    public void ShowLoseMessage()
    {
        loseMessage.SetActive(true);
    }
}

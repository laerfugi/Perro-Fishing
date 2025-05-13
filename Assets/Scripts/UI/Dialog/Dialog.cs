using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

//Dialog System. Call coroutine StartDialog with a list of lines to use the system.
public class Dialog : MonoBehaviour
{
    public static Dialog Instance { get; private set; }

    public List<string> lines;
    public bool isSpeaking;

    private int index;
    private IEnumerator coroutine;

    [Header("Dialog Window")]
    public GameObject dialogWindow;
    public TMP_Text text;
    public GameObject cursor;

    [Header("Settings")]
    public float textSpeed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogWindow.SetActive(false);
        //StartCoroutine(StartDialog(lines));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isSpeaking)
        {
            if (text.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopCoroutine(coroutine);
                text.text = lines[index];
                cursor.SetActive(true);
            }
        }
    }

    //Use this to Start Dialog
    public IEnumerator StartDialog(List<string> _lines)
    {
        EventManager.OnOpenMenuEvent();

        //set vars
        isSpeaking = true;
        index = 0;
        lines = _lines;

        //set dialog window
        dialogWindow.SetActive(true);
        text.text = "";
        cursor.SetActive(false);

        coroutine = TypeLine();
        StartCoroutine(coroutine);

        yield return new WaitUntil(() => !isSpeaking);
        EventManager.OnCloseMenuEvent();
    }

    IEnumerator TypeLine()
    {
        foreach(char c in lines[index])
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        cursor.SetActive(true);
    }

    void NextLine()
    {
        cursor.SetActive(false);
        if (index < lines.Count - 1)
        {
            index++;
            text.text = "";
            coroutine = TypeLine();
            StartCoroutine(coroutine);
        }
        else
        {
            EndDialog();
        }
    }

    void EndDialog()
    {
        EventManager.OnOpenMenuEvent();

        isSpeaking = false;
        dialogWindow.SetActive(false);
    }
}

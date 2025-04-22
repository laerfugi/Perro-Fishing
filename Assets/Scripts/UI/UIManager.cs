using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton containing references to the Player and UI elements
//Toggles UI elements via events 

//also contains bool menuIsOpen to track cursor visibility

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Player")]
    public GameObject player;

    [Header("Menu Is Open")]
    public bool menuIsOpen;     //please update this in a UI menu's logic

    [Header("UI Menus")]
    public GameObject HUD;
    public GameObject tabMenu;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.PlayerStateEvent += UpdateHUD;
    }

    private void OnDisable()
    {
        EventManager.PlayerStateEvent -= UpdateHUD;
    }

    private void Update()
    {
        CheckCursor();
    }

    void CheckCursor()
    {
        if (menuIsOpen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    void UpdateHUD(PlayerState playerState)
    {
        if (playerState == PlayerState.Active)
        {
            HUD.SetActive(true);
        }
        else
        {
            HUD.SetActive(false);
        }
    }
}

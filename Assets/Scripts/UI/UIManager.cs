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
    public bool menuIsOpen;     //public bool to tell if a menu is open

    [Header("UI Elements")]
    public GameObject PlayerHUD;
    public GameObject LittleGuyHUD;
    public GameObject tabMenu;

    private void Awake()
    {
        Instance = this;

        CheckCursor();

        //set ui's active if you want to disable them in scene
        gameObject.SetActive(true);
        PlayerHUD.SetActive(true);
        LittleGuyHUD.SetActive(false);
    }

    private void OnEnable()
    {
        EventManager.OpenMenuEvent += MenuOpen;
        EventManager.CloseMenuEvent += MenuClose;

        EventManager.PlayerStateEvent += UpdatePlayerHUD;
        EventManager.LittleGuyStateEvent += UpdateLittleGuyHUD;
    }

    private void OnDisable()
    {
        EventManager.OpenMenuEvent -= MenuOpen;
        EventManager.CloseMenuEvent -= MenuClose;

        EventManager.PlayerStateEvent -= UpdatePlayerHUD;
        EventManager.LittleGuyStateEvent -= UpdateLittleGuyHUD;
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

    //update menuIsOpen
    void MenuOpen()
    {
        menuIsOpen = true;
        CheckCursor();
    }

    void MenuClose()
    {
        menuIsOpen = false;
        CheckCursor();
    }

    //Update HUD calls
    void UpdatePlayerHUD(PlayerState state)
    {
        if (state == PlayerState.Active)
        {
            PlayerHUD.SetActive(true);
        }
        else
        {
            PlayerHUD.SetActive(false);
        }
    }

    void UpdateLittleGuyHUD(LittleGuyState state)
    {
        if (state == LittleGuyState.Active)
        {
            LittleGuyHUD.SetActive(true);
        }
        else
        {
            LittleGuyHUD.SetActive(false);
        }
    }
}

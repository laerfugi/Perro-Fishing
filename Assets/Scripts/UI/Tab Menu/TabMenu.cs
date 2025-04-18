using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//The Tab Menu GameObject will hold all menus inside it. This script will open/close the menu.
public class TabMenu : MonoBehaviour
{
    [Header("Tab Menu")]
    public GameObject menu;
    public bool isActive;

    [Header("Submenu")]
    public GameObject activeSubmenu;        //please have 1 submenu active on start and place it here

    // Start is called before the first frame update
    void Start()
    {
        //in case you forget to set the menu inactive in play mode
        isActive = menu.activeSelf;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindWithTag("Player").GetComponent<Player>().state == PlayerState.Active) {      //only execute when player is active
            if (Input.GetKeyDown(KeyCode.Tab)) { 
                isActive = !isActive; 
            }

            CheckState();
        }
    }

    //opens/closes menu and sets player settings based on isActive
    void CheckState()
    {
        if (isActive) {
            menu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            GameObject.FindWithTag("Player").GetComponent<Player>().state = PlayerState.Inactive;
        } 
        else
        {
            menu.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            GameObject.FindWithTag("Player").GetComponent<Player>().state = PlayerState.Active;
        }
    }

    //used by exit button
    public void Exit()
    {
        isActive = !isActive;
    }

    //used by section buttons
    public void OpenSubmenu(GameObject menu) {
        activeSubmenu.SetActive(false);
        menu.SetActive(true);
        activeSubmenu = menu;
    }
}

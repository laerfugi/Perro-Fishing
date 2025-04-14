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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) { isActive = !isActive; }
        CheckState();

        //in case you forget to set the menu inactive in play mode
        isActive = menu.activeSelf;
    }

    //opens/closes menu and sets player settings based on isActive
    void CheckState()
    {
        if (isActive) {
            menu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            //NEED TO CHANGE THIS WHEN WE DEFINE PLAYER STATES FOR MENU
            //disable player movement
            GameObject.FindWithTag("Player").GetComponent<Player>().enabled = false;        //disable player input movement 
            GameObject.FindWithTag("Player").GetComponentInChildren<CameraPivot>().enabled = false;     //disable camera movement
        } 
        else
        {
            menu.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            //enable player movement
            GameObject.FindWithTag("Player").GetComponent<Player>().enabled = true;         //enable player input movement 
            GameObject.FindWithTag("Player").GetComponentInChildren<CameraPivot>().enabled = true;     //enable camera movement
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

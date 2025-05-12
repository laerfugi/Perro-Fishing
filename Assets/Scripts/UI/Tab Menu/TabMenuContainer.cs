using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//The Tab Menu Container GameObject will hold all menus inside it. This script will open/close the menu.
public class TabMenuContainer : MenuClass
{
    [Header("Tab Menus")]
    public GameObject currentTabMenu;

    public List<GameObject> tabMenus;

    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);

        foreach(var tabMenu in tabMenus)
        {
            tabMenu.SetActive(false);
        }

        currentTabMenu = tabMenus[0];
        currentTabMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //when can we press tab to open menu?
        if (GameObject.FindWithTag("Player").GetComponent<Player>().state == PlayerState.Active) {      
            if (Input.GetKeyDown(KeyCode.Tab)) {
                ToggleMenu();
            }
        }
    }

    //used by exit button
    public void Exit()
    {
        ToggleMenu();
    }

    //used by tab buttons
    public void OpenTabMenu(GameObject menu) 
    {
        if (menu != currentTabMenu)
        {
            currentTabMenu.SetActive(false);
            menu.SetActive(true);
            currentTabMenu = menu;
        }
    }
}

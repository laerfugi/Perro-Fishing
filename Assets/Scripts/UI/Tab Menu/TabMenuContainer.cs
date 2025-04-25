using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//The Tab Menu Container GameObject will hold all menus inside it. This script will open/close the menu.
public class TabMenuContainer : MonoBehaviour
{
    public GameObject menu;
    public bool isActive;

    [Header("Current Tab Menu")]
    public GameObject activeTabMenu;        //please have 1 tab menu active on start and place it here

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        if (menu.activeSelf) { menu.SetActive(false); }
    }

    // Update is called once per frame
    void Update()
    {
        //when can we press tab to open menu?
        if (GameObject.FindWithTag("Player").GetComponent<Player>().state == PlayerState.Active || isActive) {      
            if (Input.GetKeyDown(KeyCode.Tab)) { 
                isActive = !isActive;
                StartCoroutine(CheckState());
            }
        }
    }

    IEnumerator CheckState()
    {
        //open menu
        if (isActive) {
            UIManager.Instance.menuIsOpen = true;
            menu.SetActive(true);

            EventManager.OnMenuEvent();
            //GameObject.FindWithTag("Player").GetComponent<Player>().ChangeState(PlayerState.Menu);
        } 
        //close menu
        else
        {
            UIManager.Instance.menuIsOpen = false;
            menu.SetActive(false);

            yield return null;
            EventManager.OnMenuEvent();
            //GameObject.FindWithTag("Player").GetComponent<Player>().ChangeState(PlayerState.Active);
        }
    }

    //used by exit button
    public void Exit()
    {
        isActive = false;
        StartCoroutine(CheckState());
    }

    //used by tab buttons
    public void OpenTabMenu(GameObject menu) {
        activeTabMenu.SetActive(false);
        menu.SetActive(true);
        activeTabMenu = menu;
    }
}

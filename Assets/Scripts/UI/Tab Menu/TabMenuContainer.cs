using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//The Tab Menu Container GameObject will hold all menus inside it. This script will open/close the menu.
public class TabMenuContainer : MonoBehaviour
{
    [Header("Tab Menu Container")]
    public GameObject menu;
    public bool isActive;

    [Header("TabMenu")]
    public GameObject activeTabMenu;        //please have 1 tab menu active on start and place it here

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindWithTag("Player").GetComponent<Player>().state == PlayerState.Active || isActive) {      //only execute when player or container menu is active
            if (Input.GetKeyDown(KeyCode.Tab)) { 
                isActive = !isActive;
                StartCoroutine(CheckState());
            }
        }
    }

    //opens/closes menu and sets player settings based on isActive
    IEnumerator CheckState()
    {
        if (isActive) {
            UIManager.Instance.menuIsOpen = true;
            menu.SetActive(true);

            GameObject.FindWithTag("Player").GetComponent<Player>().ChangeState(PlayerState.Menu);
        } 
        else
        {
            UIManager.Instance.menuIsOpen = false;
            menu.SetActive(false);

            yield return null;
            GameObject.FindWithTag("Player").GetComponent<Player>().ChangeState(PlayerState.Active);
        }
    }

    //used by exit button
    public void Exit()
    {
        isActive = false;
        StartCoroutine(CheckState());
    }

    //used by section buttons
    public void OpenTabMenu(GameObject menu) {
        activeTabMenu.SetActive(false);
        menu.SetActive(true);
        activeTabMenu = menu;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class for menu. Contains ToggleMenu method to open/close menu
public class MenuClass : MonoBehaviour
{
    public GameObject menu;
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        menu.SetActive(false);
    }

    protected void ToggleMenu()
    {
        //open menu
        if (!isActive && UIManager.Instance.menuIsOpen == false)
        {
            UIManager.Instance.menuIsOpen = true;
            isActive = true;
            menu.SetActive(true);

            EventManager.OnMenuEvent();
        }

        //close menu
        else if (isActive && UIManager.Instance.menuIsOpen == true)
        {
            StartCoroutine(CloseMenu());
        }
    }

    //delayed event call to prevent mouse click being registered after closing menu
    IEnumerator CloseMenu()
    {
        UIManager.Instance.menuIsOpen = false;
        isActive = false;
        menu.SetActive(false);

        yield return null;
        EventManager.OnMenuEvent();
    }
}

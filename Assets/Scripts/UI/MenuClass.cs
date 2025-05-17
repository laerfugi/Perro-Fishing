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
            isActive = true;
            menu.SetActive(true);

            EventManager.OnOpenMenuEvent();
        }

        //close menu
        else if (isActive && UIManager.Instance.menuIsOpen == true)
        {
            StartCoroutine(CloseMenu());
        }

        AudioManager.Instance.PlaySound("ButtonPress");
    }

    //delayed event call to prevent mouse click being registered after closing menu
    IEnumerator CloseMenu()
    {
        isActive = false;
        menu.SetActive(false);

        yield return null;
        EventManager.OnCloseMenuEvent();
    }
}

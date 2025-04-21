using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractNotification : MonoBehaviour
{
    public GameObject notificationPanel;
    public TMP_Text text;

    private void OnEnable()
    {
        EventManager.PlayerCanInteractEvent += DisplayNotification;
        EventManager.PlayerCannotInteractEvent += HideNotification;
    }
    private void OnDisable()
    {
        EventManager.PlayerCanInteractEvent -= DisplayNotification;
        EventManager.PlayerCannotInteractEvent -= HideNotification;
    }

    public void DisplayNotification(string message)
    {
        notificationPanel.SetActive(true);
        text.text = message;
    }

    void HideNotification()
    {
        notificationPanel.SetActive(false);
        text.text = "";
    }
}

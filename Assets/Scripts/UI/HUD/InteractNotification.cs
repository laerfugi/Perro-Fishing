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
        EventManager.CanInteractEvent += DisplayNotification;
        EventManager.CannotInteractEvent += HideNotification;
    }
    private void OnDisable()
    {
        EventManager.CanInteractEvent -= DisplayNotification;
        EventManager.CannotInteractEvent -= HideNotification;
    }

    private void Start()
    {
        notificationPanel.SetActive(false);
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

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryNotification : MonoBehaviour
{
    [Header("Notification")]
    public GameObject notification;
    public CanvasGroup canvasGroup;
    new public TMP_Text name;
    public Image icon;

    [Header("Fade Out Time")]
    public float slideInTime;
    public float fadeOutTime;
    private IEnumerator coroutine;

    private void OnEnable()
    {
        EventManager.InventoryAddEvent += Display;
    }
    private void OnDisable()
    {
        EventManager.InventoryAddEvent -= Display;

        notification.SetActive(false);
    }

    private void Start()
    {
        notification.SetActive(false);
    }

    //display a notification
    void Display(ItemData itemData)
    {
        canvasGroup.enabled = true;
        Reset();
        name.text = itemData.name;
        if (itemData.icon != null) { icon.sprite = itemData.icon; icon.preserveAspect = true; }

        coroutine = Animate();
        StartCoroutine(coroutine);
    }

    private void Reset()
    {
        if (coroutine != null) { StopCoroutine(coroutine); }
        notification.SetActive(true);
        canvasGroup.alpha = 1.0f;
        name.text = "";
        icon.sprite = null;
    }

    IEnumerator Animate()
    {
        //slide in
        canvasGroup.gameObject.transform.position += new Vector3(-150, 0);
        float timeElapsed = 0;
        while (timeElapsed < slideInTime)
        {
            canvasGroup.gameObject.transform.position = new Vector3(-150 + 150*(timeElapsed / slideInTime), canvasGroup.gameObject.transform.position.y);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        //fade out
        timeElapsed = 0;
        while (timeElapsed < fadeOutTime)
        {
            canvasGroup.alpha = (fadeOutTime - timeElapsed)/fadeOutTime;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        notification.SetActive(false);
        canvasGroup.alpha = 0;
    }
}

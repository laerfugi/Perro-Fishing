using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryNotification : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    new public TMP_Text name;
    public Image icon;

    [Header("Fade Out Time")]
    public float fadeOutTime;

    private void OnEnable()
    {
        EventManager.InventoryAddEvent += Display;
    }
    private void OnDisable()
    {
        EventManager.InventoryAddEvent -= Display;
    }

    private void Start()
    {
        canvasGroup.alpha = 0;
    }

    //display a notification
    void Display(ItemData itemData)
    {
        canvasGroup.enabled = true;
        Reset();
        name.text = itemData.name;
        if (itemData.icon != null) { icon.sprite = itemData.icon; }
        StartCoroutine(FadeOut());
    }

    private void Reset()
    {
        StopAllCoroutines();
        canvasGroup.alpha = 1.0f;
        name.text = "";
        icon = null;


        Debug.Log("stopped coroutine");
    }

    IEnumerator FadeOut()
    {
        Debug.Log("starting fadeout");
        yield return new WaitForSeconds(2f);

        float timeElapsed = 0;
        while (timeElapsed < fadeOutTime)
        {
            canvasGroup.alpha = (fadeOutTime - timeElapsed)/fadeOutTime;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0;
    }
}

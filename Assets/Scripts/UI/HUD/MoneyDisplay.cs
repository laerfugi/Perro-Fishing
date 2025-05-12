using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    public TMP_Text countText;
    public TMP_Text differenceText;

    public float fadeOutTime;
    private Vector3 defaultDifferenceTextPosition;

    private void OnEnable()
    {
        EventManager.MoneyEvent += UpdateDisplay;
    }
    private void OnDisable()
    {
        EventManager.MoneyEvent -= UpdateDisplay;
    }

    private void Start()
    {
        countText.text = PlayerInventory.Instance.money.ToString();
        differenceText.alpha = 0;
        defaultDifferenceTextPosition = differenceText.rectTransform.localPosition;
    }

    void UpdateDisplay(int amount)
    {
        countText.text = PlayerInventory.Instance.money.ToString();

        StopAllCoroutines();
        StartCoroutine(Animate(amount));

    }

    IEnumerator Animate(int amount)
    {
        Vector3 startPos = defaultDifferenceTextPosition;
        Vector3 endPos = defaultDifferenceTextPosition;

        if (amount > 0)
        {
            differenceText.color = Color.green;
            differenceText.text = "+" + amount.ToString();
            endPos = new Vector3(defaultDifferenceTextPosition.x, defaultDifferenceTextPosition.y + 10);
        }
        else if (amount < 0)
        {
            differenceText.color = Color.red;
            differenceText.text = "-" + amount.ToString();
            startPos = new Vector3(defaultDifferenceTextPosition.x, defaultDifferenceTextPosition.y + 10);
        }

        float timeElapsed = 0;

        differenceText.rectTransform.localPosition = defaultDifferenceTextPosition;
        while (timeElapsed < fadeOutTime)
        {
            differenceText.alpha = Mathf.Lerp(1, 0, timeElapsed / fadeOutTime);
            differenceText.rectTransform.localPosition = Vector3.Lerp(startPos, endPos, timeElapsed / fadeOutTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //reset
        differenceText.rectTransform.localPosition = defaultDifferenceTextPosition;
    }
}

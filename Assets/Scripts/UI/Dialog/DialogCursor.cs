using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogCursor : MonoBehaviour
{
    private Image cursor;

    void Awake()
    {
        cursor = GetComponent<Image>();
    }

    private void OnEnable()
    {
        cursor.enabled = true;
        InvokeRepeating("Blink", 1f, 1f);
    }

    private void OnDisable()
    {
        CancelInvoke();
        cursor.enabled = false;
    }

    void Blink()
    {
        cursor.enabled = !cursor.isActiveAndEnabled;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    //visuals
    public Image border;
    public Image image;
    public Button button;
    public TMP_Text countText;
    public Image mark;

    //content
    public ItemDataWrapper itemDataWrapper;

    private void Update()
    {
        image.preserveAspect = true;
    }
}

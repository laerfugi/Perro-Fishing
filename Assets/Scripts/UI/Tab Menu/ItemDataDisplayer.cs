using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//given itemdata in DisplayInfo(), displays the itemdata.

public class ItemDataDisplayer : MonoBehaviour
{

    new public TMP_Text name;
    public TMP_Text description;
    public Image icon;
    public GameObject item;

    private void OnEnable()
    {
        ResetInfo();
    }

    //used as button onclick() listener by the buttons spawned by InventoryMenu.cs
    public void DisplayInfo(ItemData itemdata)
    {
        name.text = itemdata.name;
        description.text = itemdata.description;
        
        if (itemdata.icon != null) { icon.sprite = itemdata.icon; }
        if (itemdata.item != null) { item = itemdata.item; }
    }

    public void ResetInfo()
    {
        name.text = "";
        description.text = "";
        icon.sprite = null;
        item = null;
    }
}


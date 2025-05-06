using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//given ItemDataWrapper in DisplayInfo(), displays the itemdata.

public class ItemDataDisplayer : MonoBehaviour
{
    [Header("ItemData Fields")]
    new public TMP_Text name;
    public TMP_Text description;
    public Image icon;
    public GameObject item;

    [Header("Extra text")]
    public TMP_Text extraText;

    [Header("Buttons")]
    public GameObject buttons;
    public Button leftButton;
    public Button rightButton;

    private void OnEnable()
    {
        ResetInfo();
    }

    //used as button onclick() listener by the buttons spawned by InventoryMenu.cs
    public void DisplayInfo(ItemDataWrapper itemDataWrapper)
    {
        //default ItemData case
        ItemData itemData = itemDataWrapper.itemData;

        name.text = itemData.name;
        description.text = itemData.description;
        
        if (itemData.icon != null) { icon.sprite = itemData.icon; }
        if (itemData.item != null) { item = itemData.item; }

        //fish_itemData
        if (itemData is Fish_ItemData)
        {
            Fish_ItemData fish_ItemData = itemData as Fish_ItemData;
            extraText.text = "$" + fish_ItemData.price.ToString();
        }
        //LittleGuy_ItemDataWrapper
        else if (itemDataWrapper is LittleGuy_ItemDataWrapper)
        {
            LittleGuy_ItemDataWrapper littleGuy_ItemDataWrapper = itemDataWrapper as LittleGuy_ItemDataWrapper;
            extraText.text = "Equipped: " + littleGuy_ItemDataWrapper.equipped.ToString();

            buttons.SetActive(true);
            leftButton.gameObject.SetActive(true); rightButton.gameObject.SetActive(false);
            leftButton.GetComponentInChildren<TMP_Text>().text = "Set as Bait";
            leftButton.onClick.AddListener(() => SetAsBait(littleGuy_ItemDataWrapper));
        }
    }

    public void ResetInfo()
    {
        name.text = "";
        description.text = "";
        icon.sprite = null;
        item = null;

        extraText.text = "";

        buttons.SetActive(false);
    }

    public void SetAsBait(LittleGuy_ItemDataWrapper littleGuy_ItemDataWrapper)
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().fishingPole.littleGuy_ItemDataWrapper.equipped = false;
        GameObject.FindWithTag("Player").GetComponent<Player>().fishingPole.littleGuy_ItemDataWrapper = littleGuy_ItemDataWrapper;
        littleGuy_ItemDataWrapper.equipped = true;

        DisplayInfo(littleGuy_ItemDataWrapper);

    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EquippedLittleGuyDisplayer : MonoBehaviour
{
    public ItemDataDisplayer itemDataDisplayer;
    public InventoryButton inventoryButton;

    private void Update()
    {
        Display();
    }

    void Display()
    {
        LittleGuy_ItemDataWrapper littleGuy_ItemDataWrapper = GameObject.FindWithTag("Player").GetComponentInChildren<FishingPole>().littleGuy_ItemDataWrapper;

        inventoryButton.image.sprite = littleGuy_ItemDataWrapper.itemData.icon;
        inventoryButton.countText.gameObject.SetActive(false);
        inventoryButton.border.color = Color.yellow;
        inventoryButton.button.onClick.AddListener(() => itemDataDisplayer.DisplayInfo(littleGuy_ItemDataWrapper));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EquippedLittleGuyDisplayer : MonoBehaviour
{
    FishingPole fishingPole;

    public ItemDataDisplayer itemDataDisplayer;
    public InventoryButton inventoryButton;

    private void Awake()
    {
        fishingPole = GameObject.FindWithTag("Player").GetComponentInChildren<FishingPole>();
    }

    private void OnEnable()
    {
        EventManager.InventoryAddEvent += Display;

        Display(null);
    }

    private void OnDisable()
    {
        EventManager.InventoryAddEvent -= Display;
    }

    void Display(ItemData itemData)
    {
        if (fishingPole.littleGuy_ItemDataWrapper != null)
        {
            inventoryButton.image.sprite = fishingPole.littleGuy_ItemDataWrapper.itemData.icon;
            inventoryButton.countText.gameObject.SetActive(false);
            inventoryButton.border.color = Color.yellow;
            inventoryButton.button.onClick.AddListener(() => itemDataDisplayer.DisplayInfo(fishingPole.littleGuy_ItemDataWrapper));
        }
    }
}

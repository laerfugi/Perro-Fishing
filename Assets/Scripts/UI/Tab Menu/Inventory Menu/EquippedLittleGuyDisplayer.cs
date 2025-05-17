using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EquippedLittleGuyDisplayer : MonoBehaviour
{
    public FishingPole fishingPole;

    public ItemDataDisplayer itemDataDisplayer;
    public InventoryButton inventoryButton;

    private void Awake()
    {
        //fishingPole = GameObject.FindWithTag("Player").GetComponentInChildren<FishingPole>();
    }

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
        Display(null);
    }

    void Display(ItemData itemData)
    {
        if (fishingPole.baitLittleGuy_ItemDataWrapper.itemData != null)
        {
            Debug.Log("frog");
            inventoryButton.image.sprite = fishingPole.baitLittleGuy_ItemDataWrapper.itemData.icon;
            inventoryButton.countText.gameObject.SetActive(false);
            inventoryButton.border.color = Color.yellow;
            inventoryButton.button.onClick.AddListener(() => itemDataDisplayer.DisplayInfo(fishingPole.baitLittleGuy_ItemDataWrapper));
            inventoryButton.button.onClick.AddListener(()=> AudioManager.Instance.PlaySound("ButtonPress"));
        }
    }
}

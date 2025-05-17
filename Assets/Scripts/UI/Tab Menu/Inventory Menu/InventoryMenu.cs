using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

//displays a menu of items in player inventory based on enum itemType.
//The menu contains buttons which contain references to each item.

public class InventoryMenu : MonoBehaviour
{
    public ItemType itemType;

    [Header("Inventory")]
    public PlayerInventory inventory;

    [Header("Menu")]
    public GameObject menuContent;  //where to spawn the buttons

    [Header("Item Data Displayer")]
    public ItemDataDisplayer itemDataDisplayer;

    [Header("Buttons")]
    public List<GameObject> inventoryButtons;
    public InventoryButton selectedButton;
    public InventoryButton markedButton;

    public ItemDataWrapper previousSelectedItemDataWrapper;
    

    [Header("Button Prefab")]
    public GameObject button;       //represents each item

    private void OnEnable()
    {
        //in case if inventory updates in menu
        EventManager.InventoryAddEvent += LoadMenu;
        EventManager.InventoryRemoveEvent += LoadMenu;

        LoadMenu(null);
        selectedButton = null;
    }
    private void OnDisable()
    {
        //in case if inventory updates in menu
        EventManager.InventoryAddEvent -= LoadMenu;    
        EventManager.InventoryRemoveEvent -= LoadMenu;
    }

    private void Start()
    {
        if (inventory == null) { inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>(); }      //reference the player
    }

    private void Update()
    {
        if (selectedButton) selectedButton.border.color = Color.green;
        if (markedButton) markedButton.mark.color = Color.yellow;
    }

    void LoadMenu(ItemData changedItem)  //changedItem is not needed but event requires the parameter
    {
        if (inventoryButtons.Count > 0)
        {
            
            foreach (GameObject button in inventoryButtons)
            {
                Destroy(button);
            }
            
        }
        inventoryButtons.Clear();

        List<ItemDataWrapper> chosenList = new List<ItemDataWrapper>();
        if (itemType == ItemType.Item || itemType == ItemType.Material)
        {
            chosenList = inventory.itemInventoryList;
            CreateItemMenuButtons(chosenList);
        }
        else if (itemType == ItemType.Fish)
        {
            chosenList = inventory.fishInventoryList;
            CreateItemMenuButtons(chosenList);
        }
        else if (itemType == ItemType.LittleGuy)
        {
            chosenList = inventory.littleGuyInventoryList.Cast<ItemDataWrapper>().ToList();     //this is so bad
            CreateLittleGuyMenuButtons(chosenList);
        }
    }

    //item and fish menu
    void CreateItemMenuButtons(List<ItemDataWrapper> chosenList)
    {
        foreach (ItemDataWrapper itemDataWrapper in chosenList)
        {
            GameObject newButton = Instantiate(button, menuContent.transform);
            inventoryButtons.Add(newButton);
            newButton.name = itemDataWrapper.itemData.name;
            newButton.GetComponent<InventoryButton>().image.sprite = itemDataWrapper.itemData.icon;
            newButton.GetComponent<InventoryButton>().countText.text = itemDataWrapper.count.ToString();
            newButton.GetComponent<InventoryButton>().button.onClick.AddListener(() => { itemDataDisplayer.DisplayInfo(itemDataWrapper); });
            newButton.GetComponent<InventoryButton>().button.onClick.AddListener(() => { SetSelectedButton(newButton.GetComponent<InventoryButton>()); });
            newButton.GetComponent<InventoryButton>().itemDataWrapper = itemDataWrapper;

            if (itemDataWrapper == previousSelectedItemDataWrapper) { SetSelectedButton(newButton.GetComponent<InventoryButton>()); }
        }
    }

    //little guy menu
    void CreateLittleGuyMenuButtons(List<ItemDataWrapper> chosenList)
    {
        foreach (ItemDataWrapper itemDataWrapper in chosenList)
        {
            GameObject newButton = Instantiate(button, menuContent.transform);
            inventoryButtons.Add(newButton);
            newButton.name = itemDataWrapper.itemData.name;
            newButton.GetComponent<InventoryButton>().image.sprite = itemDataWrapper.itemData.icon;
            newButton.GetComponent<InventoryButton>().countText.gameObject.SetActive(false);    //should i make a new button prefab for little guys?
            newButton.GetComponent<InventoryButton>().button.onClick.AddListener(() => { itemDataDisplayer.DisplayInfo(itemDataWrapper); });
            newButton.GetComponent<InventoryButton>().button.onClick.AddListener(() => { SetSelectedButton(newButton.GetComponent<InventoryButton>()); });
            newButton.GetComponent<InventoryButton>().itemDataWrapper = itemDataWrapper;

            if ((itemDataWrapper as LittleGuy_ItemDataWrapper) == GameObject.FindWithTag("Player").GetComponentInChildren<FishingPole>().baitLittleGuy_ItemDataWrapper)
            {
                markedButton = newButton.GetComponent<InventoryButton>();
            }

            if (itemDataWrapper == previousSelectedItemDataWrapper) {SetSelectedButton(newButton.GetComponent<InventoryButton>()); }
        }
    }

    void SetSelectedButton(InventoryButton inventoryButton)
    {
        if (selectedButton) {selectedButton.border.color = Color.clear; }

        selectedButton = inventoryButton;
        previousSelectedItemDataWrapper = inventoryButton.itemDataWrapper;

        AudioManager.Instance.PlaySound("ButtonPress");
    }
}

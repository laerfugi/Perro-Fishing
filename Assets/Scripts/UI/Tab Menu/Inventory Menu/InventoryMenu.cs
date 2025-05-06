using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

//displays a menu of objects in player inventory based on enum MenuType.
//The menu contains buttons which contain references to each item.

public enum MenuType {ItemMenu,FishMenu,LittleGuyMenu}

public class InventoryMenu : MonoBehaviour
{
    public MenuType menuType;

    [Header("Inventory")]
    public PlayerInventory inventory;

    [Header("Menu")]
    public GameObject menu;
    public GameObject menuContent;  //where to spawn the buttons

    [Header("Item Data Displayer")]
    public ItemDataDisplayer itemDataDisplayer;

    [Header("Buttons")]
    public GameObject button;       //button prefab to represent each item
    public InventoryButton selectedButton;
    public List<GameObject> inventoryButtons;

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
        if (menuType == MenuType.ItemMenu)
        {
            chosenList = inventory.itemInventoryList;
            CreateItemMenuButtons(chosenList);
        }
        else if (menuType == MenuType.FishMenu)
        {
            chosenList = inventory.fishInventoryList;
            CreateItemMenuButtons(chosenList);
        }
        else if (menuType == MenuType.LittleGuyMenu)
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
            newButton.GetComponent<InventoryButton>().button.onClick.AddListener(() => { itemDataDisplayer.DisplayInfo(itemDataWrapper); });
            newButton.GetComponent<InventoryButton>().button.onClick.AddListener(() => { SetSelectedButton(newButton.GetComponent<InventoryButton>()); });
            newButton.GetComponent<InventoryButton>().countText.text = itemDataWrapper.count.ToString();
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
            newButton.GetComponent<InventoryButton>().button.onClick.AddListener(() => { itemDataDisplayer.DisplayInfo(itemDataWrapper); });
            newButton.GetComponent<InventoryButton>().button.onClick.AddListener(() => { SetSelectedButton(newButton.GetComponent<InventoryButton>()); });
            newButton.GetComponent<InventoryButton>().countText.gameObject.SetActive(false);    //should i make a new button prefab for little guys?
        }
    }

    void SetSelectedButton(InventoryButton inventoryButton)
    {
        if (selectedButton) { selectedButton.border.color = Color.clear; }

        selectedButton = inventoryButton;
        selectedButton.border.color = Color.green;
    }
}

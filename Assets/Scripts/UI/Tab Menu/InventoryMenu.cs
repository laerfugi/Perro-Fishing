using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

//displays a menu of all items in a player's Item or Fish inventory based on enum MenuType.
//The menu contains buttons which contain references to each item.

public enum MenuType {ItemMenu,FishMenu}

public class InventoryMenu : MonoBehaviour
{
    public MenuType menuType;

    [Header("Inventory")]
    public PlayerInventory inventory;

    [Header("Menu")]
    public GameObject menu;
    public GameObject menuContent;  //where to spawn the buttons

    [Header("Buttons")]
    public GameObject button;       //button prefab to represent each item
    public List<GameObject> inventoryButtons;

    [Header("Item Info Displayer")]
    public ItemInfoDisplayer itemInfoDisplayer;

    private void OnEnable()
    {
        EventManager.InventoryAddEvent += LoadMenu;    //in case if inventory updates in menu

        LoadMenu(null);//need to change
    }
    private void OnDisable()
    {
        EventManager.InventoryAddEvent -= LoadMenu;    //in case if inventory updates in menu
    }

    private void Start()
    {
        if (inventory == null) { inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>(); }      //reference the player
    }

    private void Update()
    {

    }

    void LoadMenu(ItemData thingy)  //need to change
    {

        if (inventoryButtons.Count > 0)
        {
            foreach (GameObject button in inventoryButtons)
            {
                Destroy(button);
            }
        }

        List<ItemData> chosenList = new List<ItemData>();
        if (menuType == MenuType.ItemMenu)
        {
            chosenList = inventory.itemInventoryList;
        }
        else if (menuType == MenuType.FishMenu)
        {
            chosenList = inventory.fishInventoryList.Cast<ItemData>().ToList();     //cast back to ItemData
        }

        foreach (ItemData itemData in chosenList)
            {
                GameObject newButton = Instantiate(button, menuContent.transform);
                inventoryButtons.Add(newButton);
                newButton.name = itemData.name;
                newButton.GetComponent<Image>().sprite = itemData.icon;
                newButton.GetComponent<Button>().onClick.AddListener(() => { itemInfoDisplayer.DisplayInfo(itemData); });
            }
    }

}

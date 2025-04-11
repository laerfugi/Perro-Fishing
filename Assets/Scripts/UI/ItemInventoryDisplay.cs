using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventoryDisplay : MonoBehaviour
{
    public PlayerInventory inventory;

    public GameObject menu;
    public GameObject menuContent;  //where to spawn the buttons
    public GameObject button;       //button prefab to represent each item

    public List<GameObject> inventoryButtons;

    private void OnEnable()
    {
        EventManager.InventoryEvent += LoadMenu;
    }
    private void OnDisable()
    {
        EventManager.InventoryEvent -= LoadMenu;
    }

    private void Start()
    {
        if (inventory == null) { inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>(); }      //reference the player
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab)) { menu.SetActive(!menu.activeSelf); }
    }

    void LoadMenu()
    {
        if (inventoryButtons.Count > 0)
        {
            foreach (GameObject button in inventoryButtons)
            {
                Destroy(button);
            }
        }

        foreach (ItemData itemData in inventory.itemInventoryList) {
            GameObject newButton = Instantiate(button, menuContent.transform);
            inventoryButtons.Add(newButton);
            newButton.name = itemData.name;
            newButton.GetComponent<Image>().sprite = itemData.icon;
        }
    }
}

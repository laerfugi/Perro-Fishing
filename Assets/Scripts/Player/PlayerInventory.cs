using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Contains 2 inventory lists: 1 for Items and 1 for Fish.
//Inventory Methods will automatically sort a given argument ItemData into either list. May possibly need to change this but it convenient lol
public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    [SerializeField] public List<ItemData> itemInventoryList;

    [SerializeField] public List<Fish_ItemData> fishInventoryList;

    [Header("Inventory Settings")]
    //[SerializeField] private int INVENTORY_SIZE = 3;      //we can use this later to limit inventory space but it's making an error in unity since its unused lol

    [Header("Money")]
    public int money;

    public int heldObjectIndex = -1; // back to 0-based index

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //inventoryArray = new ItemData[INVENTORY_SIZE];
    }

    #region Inventory Methods
    public void AddItem(ItemData itemData)
    {
        //Fish Case
        if (itemData.GetType() == typeof(Fish_ItemData)) { fishInventoryList.Add(itemData as Fish_ItemData); }

        //Item Case
        else { itemInventoryList.Add(itemData); }

        EventManager.OnInventoryEvent();
        /*
        for (int i = 0; i < inventoryArray.Length; i++)
        {
            if (inventoryArray[i] == null)
            {
                inventoryArray[i] = itemData;
                Debug.Log($"Added {itemData.name} to inventory.");
                return;
            }
        }
        

        EventManager.OnInventoryEvent();
        Debug.LogWarning("Inventory is full.");
        */
    }

    public void RemoveItem(ItemData itemData)
    {
        if (InInventory(itemData))
        {

            //Fish Case
            if (itemData.GetType() == typeof(Fish_ItemData)) { fishInventoryList.Remove(itemData as Fish_ItemData); }

            //Item Case
            else { itemInventoryList.Remove(itemData); }

            EventManager.OnInventoryEvent();
        }

        /*
        for (int i = 0; i < inventoryArray.Length; i++)
        {
            if (inventoryArray[i] == itemData)
            {
                inventoryArray[i] = null;
                Debug.Log($"Removed {itemData.name} from inventory.");
                return;
            }
        }
        EventManager.OnInventoryEvent();
        Debug.LogWarning("Item not found in inventory.");
        */
    }

    public bool InInventory(ItemData itemData)
    {
        //Fish Case
        if (itemData.GetType() == typeof(Fish_ItemData)) { if (fishInventoryList.Contains(itemData as Fish_ItemData)) { return true; } }

        //Item Case
        else { if (itemInventoryList.Contains(itemData)) { return true; } }

        return false;

        /*
        foreach (var item in inventoryArray)
        {
            if (item == itemData)
            {
                return true;
            }
        }

        return false;
        */
    }

    //might need this in case if we are going to limit inventory space, but not yet
    /*
    public bool IsFull()
    {
        foreach (var item in inventoryArray)
        {
            if (item == null)
            {
                return false;
            }
        }

        return true;
    }
    */
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    [SerializeField] private ItemData[] inventoryArray;

    [Header("Inventory Settings")]
    [SerializeField] private int INVENTORY_SIZE = 3;

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

        inventoryArray = new ItemData[INVENTORY_SIZE];
    }

    #region Inventory Methods
    public void AddItem(ItemData itemData)
    {
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
    }

    public void RemoveItem(ItemData itemData)
    {
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
    }

    public bool InInventory(ItemData itemData)
    {
        foreach (var item in inventoryArray)
        {
            if (item == itemData)
            {
                return true;
            }
        }

        return false;
    }

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
    #endregion
}

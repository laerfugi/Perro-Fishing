using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Contains lists for items and fish.
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
    }

    #region Inventory Methods
    public void AddItem(ItemData itemData)
    {
        //Fish Case
        if (itemData.GetType() == typeof(Fish_ItemData)) { fishInventoryList.Add(itemData as Fish_ItemData); }

        //Item Case
        else { itemInventoryList.Add(itemData); }

        EventManager.OnInventoryAddEvent(itemData);
    }

    public void RemoveItem(ItemData itemData)
    {
        if (InInventory(itemData))
        {

            //Fish Case
            if (itemData.GetType() == typeof(Fish_ItemData)) { fishInventoryList.Remove(itemData as Fish_ItemData); }

            //Item Case
            else { itemInventoryList.Remove(itemData); }

            EventManager.OnInventoryRemoveEvent(itemData);
        }
    }

    public bool InInventory(ItemData itemData)
    {
        //Fish Case
        if (itemData.GetType() == typeof(Fish_ItemData)) { if (fishInventoryList.Contains(itemData as Fish_ItemData)) { return true; } }

        //Item Case
        else { if (itemInventoryList.Contains(itemData)) { return true; } }

        return false;
    }
    #endregion
}

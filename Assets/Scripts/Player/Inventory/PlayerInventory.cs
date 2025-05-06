using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Lists representing player's inventory.
//Inventory Methods will automatically sort a given argument ItemData into either list. May possibly need to change this but it convenient lol
public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    [Header("Items and Fish Inventory")]
    [SerializeField] public List<ItemDataWrapper> itemInventoryList;
    [SerializeField] public List<ItemDataWrapper> fishInventoryList;

    [Header("Little Guy Inventory")]
    [SerializeField] public List<LittleGuy_ItemDataWrapper> littleGuyInventoryList;

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

    #region Items and Fish Inventory Methods
    public void AddItem(ItemData itemData)
    {
        ItemDataWrapper targetWrapper = InInventory(itemData);

        //if item already exists, +1 to count
        if (targetWrapper != null)
        {
            //Fish Case
            if (itemData.GetType() == typeof(Fish_ItemData))
            {
                targetWrapper.count += 1;
            }

            //Item Case
            else
            {
                targetWrapper.count += 1;
            }
        }
        //else, make new wrapper
        else
        {
            //Fish Case
            if (itemData.GetType() == typeof(Fish_ItemData))
            {
                fishInventoryList.Add(new ItemDataWrapper(itemData, 1));
            }

            //Item Case
            else
            {
                itemInventoryList.Add(new ItemDataWrapper(itemData, 1));
            }
        }
        EventManager.OnInventoryAddEvent(itemData);
    }

    public void RemoveItem(ItemData itemData)
    {
        ItemDataWrapper targetWrapper = InInventory(itemData);

        if (targetWrapper != null)
        {

            //Fish Case
            if (itemData.GetType() == typeof(Fish_ItemData)) 
            {
                targetWrapper.count -= 1; 
                if (targetWrapper.count == 0) { fishInventoryList.Remove(targetWrapper); }
            }

            //Item Case
            else
            {
                targetWrapper.count -= 1;
                if (targetWrapper.count == 0) { itemInventoryList.Remove(targetWrapper); }
            }
            EventManager.OnInventoryRemoveEvent(itemData);
        }
    }

    public ItemDataWrapper InInventory(ItemData itemData)
    {

        //Fish Case
        if (itemData.GetType() == typeof(Fish_ItemData))
        {
            foreach (ItemDataWrapper wrapper in fishInventoryList)
            {
                if (wrapper.itemData == itemData) { return wrapper; }
            }
        }

        //Item Case
        else
        {
            foreach (ItemDataWrapper wrapper in itemInventoryList)
            {
                if (wrapper.itemData == itemData) { return wrapper; }
            }
        }
        return null;
    }
    #endregion

    #region Little Guy Inventory Methods
    public void AddLittleGuy(LittleGuy littleGuy)
    {
        littleGuyInventoryList.Add(new LittleGuy_ItemDataWrapper(littleGuy.itemData, littleGuy.gameObject));
    }

    public void RemoveLittleGuy(LittleGuy littleGuy)
    {
        LittleGuy_ItemDataWrapper targetWrapper = InInventoryLittleGuy(littleGuy);

        if (targetWrapper != null)
        {
            littleGuyInventoryList.Remove(targetWrapper);
        }
    }

    public LittleGuy_ItemDataWrapper InInventoryLittleGuy(LittleGuy littleGuy)
    {
        foreach(LittleGuy_ItemDataWrapper wrapper in littleGuyInventoryList)
        {
            if (wrapper.itemData == littleGuy.itemData) { return wrapper; }
        }
        return null;
    }
    #endregion
}

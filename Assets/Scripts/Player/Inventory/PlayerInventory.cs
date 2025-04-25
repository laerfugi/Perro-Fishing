using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Contains lists for items and fish.
//Inventory Methods will automatically sort a given argument ItemData into either list. May possibly need to change this but it convenient lol
public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    [SerializeField] public List<ItemDataWrapper> itemInventoryList;

    [SerializeField] public List<ItemDataWrapper> fishInventoryList;

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
}

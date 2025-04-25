using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//contains count of an ItemData.
[System.Serializable]
public class ItemDataWrapper
{
    public ItemData itemData;
    public int count;

    public ItemDataWrapper(ItemData itemData, int count)
    {
        this.itemData = itemData;
        this.count = count;
    }
}

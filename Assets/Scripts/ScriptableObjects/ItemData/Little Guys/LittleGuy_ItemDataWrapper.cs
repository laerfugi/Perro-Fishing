using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class LittleGuy_ItemDataWrapper:ItemDataWrapper
{
    public GameObject littleGuy;    //the little guy currently in scene

    public LittleGuy_ItemDataWrapper(ItemData itemData, GameObject littleGuy) : base(itemData, 1)
    {
        this.littleGuy = littleGuy;
    }
}

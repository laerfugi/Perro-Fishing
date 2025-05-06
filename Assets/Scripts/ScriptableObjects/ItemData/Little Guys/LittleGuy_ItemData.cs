using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/ItemData/LittleGuy_ItemData")]
public class LittleGuy_ItemData : ItemData
{
    public LittleGuy_ItemData()
    {
        this.itemType = ItemType.LittleGuy;
    }
}

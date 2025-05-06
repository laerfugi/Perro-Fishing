using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/ItemData/Fish_ItemData")]
public class Fish_ItemData : ItemData
{
    [Header("Fish_ItemData")]
    public int price;
}

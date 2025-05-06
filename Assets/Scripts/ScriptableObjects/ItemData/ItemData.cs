using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType {Item,Material,Fish,LittleGuy};

//holds data of an item. Can be edited as a ScriptableObject
[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/ItemData/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Basic Data")]
    new public string name = "New Item";
    public ItemType itemType;
    public string description;
    public Sprite icon;
    public GameObject item;     //reference to prefab
}

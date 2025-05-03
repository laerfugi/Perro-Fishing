using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum itemType {Item,Material,Fish};

//holds data of an item. Can be edited as a ScriptableObject
[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Interactable/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Basic Data")]
    new public string name = "New Item";
    public itemType itemType;
    public string description;
    public Sprite icon;
    public GameObject item;     //reference to prefab
}

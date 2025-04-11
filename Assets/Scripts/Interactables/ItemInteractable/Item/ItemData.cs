using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//holds data of an item. Can be edited as a ScriptableObject
[CreateAssetMenu(fileName = "New Item", menuName = "Interactable/ItemData")]
public class ItemData : ScriptableObject
{
    new public string name = "New Item";
    public string description;
    public Sprite icon;
    public GameObject item;     //reference to prefab
}

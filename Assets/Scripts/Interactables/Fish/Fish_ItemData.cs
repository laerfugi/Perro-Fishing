using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Interactable/Fish_ItemData")]
public class Fish_ItemData : ItemData
{
    public CombinationType combinationType;
    public int price;
}

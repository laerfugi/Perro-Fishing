using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDatabase", menuName = "ScriptableObjects/Database")]
public class Database : ScriptableObject
{
    public List<ItemData> itemList;
    public List<Fish_ItemData> fishList;
    public List<LittleGuy_ItemData> littleGuyList;
    public List<MinigameData> minigameList;
}

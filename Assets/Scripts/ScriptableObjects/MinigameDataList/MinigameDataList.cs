using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMinigameDataList", menuName = "ScriptableObjects/MinigameDataList")]
public class MinigameDataList : ScriptableObject
{
    public List<MinigameData> list;
}

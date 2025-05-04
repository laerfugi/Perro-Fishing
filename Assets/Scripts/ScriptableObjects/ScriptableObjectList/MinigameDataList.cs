using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewScriptableObjectList", menuName = "ScriptableObjects/ScriptableObjectList")]
public class ScriptableObjectList : ScriptableObject
{
    public List<MinigameData> list;
}

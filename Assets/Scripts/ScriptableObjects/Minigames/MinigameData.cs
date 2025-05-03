using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewMinigameData", menuName = "ScriptableObjects/MinigameData")]
public class MinigameData : ScriptableObject
{
    public string sceneName;
    public string startMessage;
    public float maxMinigameTime;
}

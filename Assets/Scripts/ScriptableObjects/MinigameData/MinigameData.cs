using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinigameControls {Keyboard, KeyboardAndSpacebar, Mouse} //keyboard refers to wasd

[CreateAssetMenu(fileName = "NewMinigameData", menuName = "ScriptableObjects/MinigameData")]
public class MinigameData : ScriptableObject
{
    public string sceneName;
    public string startMessage;
    public float maxMinigameTime;
    public MinigameControls minigameControls;
}

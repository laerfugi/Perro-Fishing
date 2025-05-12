using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string name;
    public Fish_ItemData fishRequest;     //what to fetch
    public int amount;
    public int reward;
    public bool complete;
    public bool claimed;        //if reward has been claimed yet
}
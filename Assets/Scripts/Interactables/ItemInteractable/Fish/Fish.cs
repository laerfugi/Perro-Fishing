using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : ItemInteractable
{
    public override void Use()
    {
        Debug.Log("I am a fish");
    }
}
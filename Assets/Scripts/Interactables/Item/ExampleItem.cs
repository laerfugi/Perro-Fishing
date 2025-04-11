using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ItemInteractable
{
    public override void Use()
    {
        Debug.Log("I am being used");
    }
}

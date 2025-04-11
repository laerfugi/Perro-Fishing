using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleItem : ItemInteractable
{
    public override void Use()
    {
        Debug.Log("I am being used");
    }
}

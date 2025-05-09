using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnableItem : ItemInteractable
{
    private MaterialSpawner spawner;
    private Vector3 spawnPoint;

    public void Initialize(MaterialSpawner spawner, Vector3 spawnPoint)
    {
        this.spawner = spawner;
        this.spawnPoint = spawnPoint;
    }

    public override void Interact()
    {
        base.Interact(); 

        if (spawner != null)
        {
            Material_ItemData materialData = itemData as Material_ItemData;
            spawner.NotifyMaterialCollected(spawnPoint, materialData);
        }
    }
}
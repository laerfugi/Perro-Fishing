using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseWrapper
{
    private Database database;

    public DatabaseWrapper(Database db)
    {
        database = db;
    }

    public LittleGuy_ItemData GetLittleGuyData(CombinationType combinationType)
    {
        foreach (var littleGuy in database.littleGuyList)
        {
            if (littleGuy.type == combinationType)
            {
                return littleGuy;
            }
        }

        return null;
    }

    public Material_ItemData GetMaterialData(MaterialType materialType)
    {
        foreach (var material in database.materialList)
        {
            if (material.type == materialType)
            {
                return material;
            }
        }

        return null;
    }
}
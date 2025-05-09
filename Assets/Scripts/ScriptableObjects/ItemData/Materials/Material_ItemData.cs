using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Material", menuName = "ScriptableObjects/ItemData/Material_ItemData")]
public class Material_ItemData : ItemData
{
    [Header("Material_ItemData")]
    public MaterialType type;
}
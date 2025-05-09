using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] private Database database;
    void Start()
    {
        RecipeBook.Initialize(database);
    }

    public GameObject Craft(Material_ItemData material1, Material_ItemData material2, Vector3 spawnPosition)
    {
        if (!HasRequiredMaterials(material1, material2))
        {
            Debug.LogWarning("Missing materials.");
            return null;
        }

        LittleGuy_ItemData littleGuyData = RecipeBook.UseRecipe(material1, material2);
        // Failed recipe
        if (littleGuyData == null)
        {
            Debug.LogWarning("Invalid recipe");
            return null;
        }

        // Recipe success
        ConsumeMaterials(material1, material2);
        // Spawn little guy through giving data
        GameObject craftedItem = LittleGuySpawner.Instance.CreateLittleGuy(spawnPosition, littleGuyData);

        if (craftedItem.TryGetComponent(out LittleGuy littleGuy))
        {
            PlayerInventory.Instance.AddLittleGuy(littleGuy);
        }

        return craftedItem; // Unsure if needed
    }

    private bool HasRequiredMaterials(ItemData material1, ItemData material2)
    {
        var wrapper1 = PlayerInventory.Instance.InInventory(material1);
        var wrapper2 = PlayerInventory.Instance.InInventory(material2);

        return wrapper1 != null && wrapper1.count > 0 &&
               wrapper2 != null && wrapper2.count > 0;
    }

    private void ConsumeMaterials(ItemData material1, ItemData material2)
    {
        PlayerInventory.Instance.RemoveItem(material1);
        PlayerInventory.Instance.RemoveItem(material2);
    }

}

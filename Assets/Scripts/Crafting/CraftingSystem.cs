using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    private DatabaseWrapper databaseWrapper;
    private RecipeDisplayManager recipeDisplayManager;
    private CraftingUIManager craftingUIManager;
    public Transform spawnPoint;
    void Awake()
    {
        databaseWrapper = GetComponent<DatabaseWrapper>();
        RecipeBook.Initialize(databaseWrapper);
    }

    void Start()
    {
        recipeDisplayManager = GetComponent<RecipeDisplayManager>();
        craftingUIManager = GetComponent<CraftingUIManager>();
    }

    public void Craft()
    {
        //(Material_ItemData mat1, Material_ItemData mat2) = (recipeDisplayManager.currentCraft[0], recipeDisplayManager.currentCraft[1]);
        //Debug.Log($"{recipeDisplayManager.currentCraft}");
        if (!HasRequiredMaterials(recipeDisplayManager.currentCraft[0], recipeDisplayManager.currentCraft[1]))
        {
            Debug.LogWarning("Missing materials.");
            //return null;
            return;
        }

        LittleGuy_ItemData littleGuyData = RecipeBook.UseRecipe(recipeDisplayManager.currentCraft[0], recipeDisplayManager.currentCraft[1]);
        // Failed recipe
        if (littleGuyData == null)
        {
            Debug.LogWarning("Invalid recipe");
            //return null;
            return;
        }

        // Recipe success
        ConsumeMaterials(recipeDisplayManager.currentCraft[0], recipeDisplayManager.currentCraft[1]);
        recipeDisplayManager.OnOpen(); // refresh the UI
        craftingUIManager.ResetCraftingUI();
        // Spawn little guy through giving data
        GameObject craftedItem = LittleGuySpawner.Instance.CreateLittleGuy(spawnPoint.localPosition, littleGuyData);

        if (craftedItem.TryGetComponent(out LittleGuy littleGuy))
        {
            PlayerInventory.Instance.AddLittleGuy(littleGuy);
        }

        //return craftedItem; // Unsure if needed
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

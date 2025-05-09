using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeDisplayManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform inventoryGridView;
    public GameObject inventoryButtonPrefab;
    public Transform recipeGridView;
    public GameObject recipeButtonPrefab;

    [Header("Dependencies")]
    private PlayerInventory playerInventory;
    public CraftingUIManager craftingUIManager;

    private List<GameObject> inventoryButtons = new List<GameObject>();
    private List<GameObject> recipeButtons = new List<GameObject>();

    void Start()
    {
        playerInventory = PlayerInventory.Instance;
        UpdateInventoryDisplay();
        CreateRecipeButtons();
    }

    public void UpdateInventoryDisplay()
    {
        foreach (var button in inventoryButtons)
        {
            Destroy(button);
        }
        inventoryButtons.Clear();

        //foreach (var wrapper in playerInventory.itemInventoryList)
        //{
        //    Material_ItemData materialData = wrapper.itemData as Material_ItemData;
        //    if (materialData != null)
        //    {
        //        CreateInventoryEntry(materialData, wrapper.count);
        //    }
        //}

        //foreach (MaterialType materialType in MaterialTypeHelper.Count)
        //{
        //}
    }

    private void CreateInventoryEntry(Material_ItemData materialData, int count)
    {
        GameObject newButton = Instantiate(inventoryButtonPrefab, inventoryGridView);
        Image buttonImage = newButton.GetComponentInChildren<Image>();
        Text countText = newButton.GetComponentInChildren<Text>();

        buttonImage.sprite = materialData.icon;
        countText.text = count.ToString();

        if (count <= 0)
        {
            buttonImage.color = new Color(1f, 1f, 1f, 0.5f);
            newButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            buttonImage.color = Color.white;
            newButton.GetComponent<Button>().interactable = true;

            newButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                craftingUIManager.UpdateIngredientSlot(0, buttonImage.sprite);
            });
        }

        inventoryButtons.Add(newButton);
    }

    // Dynamically create recipes
    private void CreateRecipeButtons()
    {
        foreach (var button in recipeButtons)
        {
            Destroy(button);
        }
        recipeButtons.Clear();

        List<ItemDataRecipe> recipes = RecipeBook.GetAllRecipes();
        foreach (var recipe in recipes)
        {
            CreateRecipeButton(recipe);
        }
    }

    private void CreateRecipeButton(ItemDataRecipe recipe)
    {
        GameObject newButton = Instantiate(recipeButtonPrefab, recipeGridView);
        Image buttonImage = newButton.GetComponentInChildren<Image>();
        TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();

        buttonImage.sprite = recipe.Result.icon;
        buttonText.text = recipe.Result.name;

        // Add click listener to set the crafting slots to the recipe's materials
        newButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            craftingUIManager.ResetCraftingUI();
            craftingUIManager.UpdateIngredientSlot(0, recipe.MaterialOne.icon);
            craftingUIManager.UpdateIngredientSlot(1, recipe.MaterialTwo.icon);
        });

        recipeButtons.Add(newButton);
    }
}
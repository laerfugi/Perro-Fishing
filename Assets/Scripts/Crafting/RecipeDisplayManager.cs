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
    private DatabaseWrapper dbWrapper;

    private List<GameObject> inventoryButtons = new List<GameObject>();
    private List<GameObject> recipeButtons = new List<GameObject>();

    private Dictionary<MaterialType, int> originalCounts = new Dictionary<MaterialType, int>();
    private Dictionary<MaterialType, int> currentCounts = new Dictionary<MaterialType, int>();

    void Start()
    {
        playerInventory = PlayerInventory.Instance;
        dbWrapper = GetComponent<DatabaseWrapper>();
        UpdateInventoryDisplay();
        CreateRecipeButtons();
    }

    public void OnOpen()
    {
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

        foreach (MaterialType materialType in System.Enum.GetValues(typeof(MaterialType)))
        {
            Material_ItemData materialData = dbWrapper.GetMaterialData(materialType);

            if (materialData != null)
            {
                int count = 0;

                ItemDataWrapper item = playerInventory.InInventory(materialData);
                if (item != null) count = item.count;

                originalCounts[materialType] = count;
                currentCounts[materialType] = count;

                CreateInventoryEntry(materialData, count);
            }
            else
            {
                Debug.LogWarning($"Material_ItemData not found for MaterialType: {materialType}");
            }
        }
    }

    private void CreateInventoryEntry(Material_ItemData materialData, int count)
    {
        GameObject newButton = Instantiate(inventoryButtonPrefab, inventoryGridView, false);
        Image buttonImage = newButton.GetComponentInChildren<Image>();
        TMP_Text countText = newButton.transform.Find("Count").GetComponent<TMP_Text>();
        TMP_Text nameText = newButton.transform.Find("Name").GetComponent<TMP_Text>();

        buttonImage.sprite = materialData.icon;
        countText.text = count.ToString();
        nameText.text = materialData.name;

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
                if (currentCounts[materialData.type] > 0)
                {
                    craftingUIManager.UpdateIngredientSlot(0, buttonImage.sprite);
                    DecrementMaterialCount(materialData.type, countText);
                }
            });
        }

        inventoryButtons.Add(newButton);
    }

    private void DecrementMaterialCount(MaterialType materialType, TMP_Text countText)
    {
        if (currentCounts[materialType] > 0)
        {
            currentCounts[materialType]--;
            countText.text = currentCounts[materialType].ToString();

            // Disable the button if the count reaches 0
            if (currentCounts[materialType] <= 0)
            {
                GameObject button = inventoryButtons.Find(b =>
                    b.GetComponentInChildren<Image>().sprite == dbWrapper.GetMaterialData(materialType).icon);
                if (button != null)
                {
                    button.GetComponent<Button>().interactable = false;
                    button.GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 0.5f);
                }
            }
        }
    }

    public void ResetMaterialCounts()
    {
        foreach (var materialType in originalCounts.Keys)
        {
            currentCounts[materialType] = originalCounts[materialType];

            GameObject button = inventoryButtons.Find(b =>
                b.GetComponentInChildren<Image>().sprite == dbWrapper.GetMaterialData(materialType).icon);
            if (button != null)
            {
                TMP_Text countText = button.transform.Find("Count").GetComponent<TMP_Text>();
                countText.text = currentCounts[materialType].ToString();

                if (currentCounts[materialType] > 0)
                {
                    button.GetComponent<Button>().interactable = true;
                    button.GetComponentInChildren<Image>().color = Color.white;
                }
                else
                {
                    button.GetComponent<Button>().interactable = false;
                    button.GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 0.5f);
                }
            }
        }
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
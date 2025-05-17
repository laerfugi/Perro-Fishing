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
    public Button craftButton;
    public GameObject rateUpArea;

    [Header("Dependencies")]
    private PlayerInventory playerInventory;
    public CraftingUIManager craftingUIManager;
    private DatabaseWrapper dbWrapper;

    private List<GameObject> inventoryButtons = new List<GameObject>();
    private List<GameObject> recipeButtons = new List<GameObject>();

    private Dictionary<MaterialType, int> originalCounts = new Dictionary<MaterialType, int>();
    private Dictionary<MaterialType, int> currentCounts = new Dictionary<MaterialType, int>();

    public List<Material_ItemData> currentCraft;

    void Start()
    {
        playerInventory = PlayerInventory.Instance;
        dbWrapper = GetComponent<DatabaseWrapper>();
        OnOpen();
    }

    public void OnOpen()
    {
        ResetCraft();
        UpdateInventoryDisplay();
        CreateRecipeButtons();
    }

    // Dynamically create materials based off of mats in database
    private void UpdateInventoryDisplay()
    {
        ClearButtons(inventoryButtons);

        foreach (MaterialType materialType in System.Enum.GetValues(typeof(MaterialType)))
        {
            Material_ItemData materialData = dbWrapper.GetMaterialData(materialType);

            if (materialData != null)
            {
                int count = GetMaterialCount(materialData);
                originalCounts[materialType] = count;
                currentCounts[materialType] = count;

                CreateMaterialButton(materialData, count);
            }
            else
            {
                Debug.LogWarning($"Material_ItemData not found for MaterialType: {materialType}");
            }
        }
    }

    private void CreateMaterialButton(Material_ItemData materialData, int count)
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
            SetButtonState(newButton, buttonImage, false);
        }
        else
        {
            SetButtonState(newButton, buttonImage, true);

            newButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySound("ButtonPress");
                if (currentCounts[materialData.type] > 0 && currentCraft.Count < 2)
                {
                    AddMaterialToCraft(materialData);
                    CheckAndUpdateCraftingState();
                }
            });
        }

        // Add the button to the inventory buttons list
        inventoryButtons.Add(newButton);
    }

    // Dynamically create recipes
    private void CreateRecipeButtons()
    {
        ClearButtons(recipeButtons);

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

        newButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySound("ButtonPress");
            craftingUIManager.ResetCraftingUI();

            currentCraft.Clear();
            //currentCraft.Add(recipe.MaterialOne);
            //currentCraft.Add(recipe.MaterialTwo);
            AddMaterialToCraft(recipe.MaterialOne);
            AddMaterialToCraft(recipe.MaterialTwo);

            CheckAndUpdateCraftingState();
        });

        recipeButtons.Add(newButton);
    }

    #region Recipe Helpers
    private void AddMaterialToCraft(Material_ItemData materialData)
    {
        if (currentCraft.Count >= 2) return;

        int slot = currentCraft.Count;
        craftingUIManager.UpdateIngredientSlot(slot, materialData.icon);

        // Decrement material count and update the UI
        DecrementMaterialCount(materialData.type);

        currentCraft.Add(materialData);

        // Check if the current craft forms a valid recipe
        if (currentCraft.Count == 2)
        {
            CheckAndUpdateCraftingState();
        }
    }

    private void CheckAndUpdateCraftingState()
    {
        if (currentCraft.Count < 2) return;

        bool hasMaterialOne = originalCounts[currentCraft[0].type] > 0;
        bool hasMaterialTwo = originalCounts[currentCraft[1].type] > 0;

        craftingUIManager.craftIngredients[0].color = hasMaterialOne ? Color.white : new Color(1f, 1f, 1f, 0.5f);
        craftingUIManager.craftIngredients[1].color = hasMaterialTwo ? Color.white : new Color(1f, 1f, 1f, 0.5f);

        if (RecipeBook.IsValidRecipe(currentCraft[0], currentCraft[1]) && hasMaterialOne && hasMaterialTwo)
        {
            craftButton.image.color = Color.green; // Can craft
            craftButton.interactable = true;
        }
        else
        {
            craftButton.image.color = Color.red; // Can not craft
            craftButton.interactable = false;
        }
        // Show rate up area with according fish
        LittleGuy_ItemData result = RecipeBook.UseRecipe(currentCraft[0], currentCraft[1]);
        if (result != null)
        {
            rateUpArea.SetActive(true);
            rateUpArea.GetComponentInChildren<Image>().sprite = dbWrapper.GetFishData(result.type).icon;
            //rateUpIcon = newIcon;
        }
        else
        {
            rateUpArea.SetActive(false);
        }

    }

    private void DecrementMaterialCount(MaterialType materialType)
    {
        if (currentCounts[materialType] > 0)
        {
            currentCounts[materialType]--;

            // Update the count text in the inventory button
            GameObject button = inventoryButtons.Find(b =>
                b.GetComponentInChildren<Image>().sprite == dbWrapper.GetMaterialData(materialType).icon);
            if (button != null)
            {
                TMP_Text countText = button.transform.Find("Count").GetComponent<TMP_Text>();
                countText.text = currentCounts[materialType].ToString();

                // Disable button if count is 0
                if (currentCounts[materialType] <= 0)
                {
                    SetButtonState(button, button.GetComponentInChildren<Image>(), false);
                }
            }
        }
    }
    #endregion

    #region UI Helpers
    public void ResetCraft()
    {
        // Clear curr recipe
        currentCraft.Clear();
        //craftingUIManager.ResetCraftingUI();
        foreach (var materialType in originalCounts.Keys)
        {
            currentCounts[materialType] = originalCounts[materialType];

            GameObject button = inventoryButtons.Find(b =>
                b.GetComponentInChildren<Image>().sprite == dbWrapper.GetMaterialData(materialType).icon);
            if (button != null)
            {
                TMP_Text countText = button.transform.Find("Count").GetComponent<TMP_Text>();
                countText.text = currentCounts[materialType].ToString();

                SetButtonState(button, button.GetComponentInChildren<Image>(), currentCounts[materialType] > 0);
            }
        }
        // Reset craft button color
        craftButton.image.color = Color.gray; 
        craftButton.interactable = false;

        rateUpArea.SetActive(false);
    }

    private void ClearButtons(List<GameObject> buttons)
    {
        foreach (var button in buttons)
        {
            Destroy(button);
        }
        buttons.Clear();
    }
    private void SetButtonState(GameObject button, Image buttonImage, bool isEnabled)
    {
        // Make a button slightly transparent if not enabled
        button.GetComponent<Button>().interactable = isEnabled;
        buttonImage.color = isEnabled ? Color.white : new Color(1f, 1f, 1f, 0.5f);
    }
    private int GetMaterialCount(Material_ItemData materialData)
    {
        ItemDataWrapper item = playerInventory.InInventory(materialData);
        return item != null ? item.count : 0;
    }
    #endregion
}
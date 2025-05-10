using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIManager : MenuClass
{
    public Image[] craftIngredients;
    public GameObject canCatchArea;
    private Image canCatchPicture;
    public Sprite originalTransparent;
    private RecipeDisplayManager recipeDisplayManager;

    private Vector2 originalSize;

    void Start()
    {
        recipeDisplayManager = GetComponent<RecipeDisplayManager>();
        ResetCraftingUI();
    }

    public void ReloadCraftingUI()
    {
        recipeDisplayManager.OnOpen();
        ToggleMenu();
    }

    public void UpdateIngredientSlot(int slotIndex, Sprite materialSprite)
    {
        Image image = craftIngredients[slotIndex];
        image.sprite = materialSprite;
        image.gameObject.SetActive(true);
    }

    public void ResetCraftingUI()
    {
        craftIngredients[0].sprite = originalTransparent;
        craftIngredients[1].sprite = originalTransparent;
        craftIngredients[0].color = new Color(1f, 1f, 1f, 1f);
        craftIngredients[1].color = new Color(1f, 1f, 1f, 1f);
        recipeDisplayManager.ResetCraft();
    }

    public void ShowCraftingResult(Sprite resultSprite)
    {
        canCatchPicture.sprite = resultSprite;
        canCatchArea.SetActive(true);
    }
}

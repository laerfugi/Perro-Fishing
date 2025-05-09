using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIManager : MenuClass
{
    public Image[] craftIngredients;
    public GameObject canCatchArea;
    private Image canCatchPicture;

    private Vector2 originalSize;

    void Start()
    {
        ResetCraftingUI();
    }

    public void ShowCraftingUI()
    {
        menu.SetActive(true);
    }

    public void HideCraftingUI()
    {
        menu.SetActive(false);
    }

    public void UpdateIngredientSlot(int slotIndex, Sprite materialSprite)
    {
        Image image = craftIngredients[slotIndex];
        image.sprite = materialSprite;
        image.gameObject.SetActive(true);
    }

    public void ResetCraftingUI()
    {
        foreach (Image ingredientSlot in craftIngredients)
        {
            ingredientSlot.sprite = null;
            //ingredientSlot.gameObject.SetActive(false);
            //ingredientSlot.rectTransform.sizeDelta = originalSize;
        }

        canCatchArea.SetActive(false);
    }

    public void ShowCraftingResult(Sprite resultSprite)
    {
        canCatchPicture.sprite = resultSprite;
        canCatchArea.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIManager : MenuClass
{
    private RawImage[] craftIngredients;
    private GameObject canCatchArea;
    private Image canCatchPicture;

    private Vector2 originalSize;

    void Start()
    {
        originalSize = craftIngredients[0].GetComponent<RawImage>().rectTransform.sizeDelta;
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

    public void UpdateIngredientSlot(int slotIndex, MaterialType materialType, Sprite materialSprite)
    {
        RawImage rawImage = craftIngredients[slotIndex];
        rawImage.texture = materialSprite.texture;
        rawImage.gameObject.SetActive(true);
    }

    public void ResetCraftingUI()
    {
        foreach (RawImage ingredientSlot in craftIngredients)
        {
            ingredientSlot.texture = null;
            ingredientSlot.gameObject.SetActive(false);
            ingredientSlot.rectTransform.sizeDelta = originalSize;
        }

        canCatchArea.SetActive(false);
    }

    public void ShowCraftingResult(Sprite resultSprite)
    {
        canCatchPicture.sprite = resultSprite;
        canCatchArea.SetActive(true);
    }
}

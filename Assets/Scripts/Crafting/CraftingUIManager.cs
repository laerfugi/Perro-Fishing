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

    private Vector2 originalSize;

    void Start()
    {
        //originalTransparent = craftIngredients[0];
        ResetCraftingUI();
    }

    public void ReloadCraftingUI()
    {
        RecipeDisplayManager temp = GetComponent<RecipeDisplayManager>();
        temp.OnOpen();
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
        //foreach (Image ingredientSlot in craftIngredients)
        //{
        //    ingredientSlot.sprite = originalTransparent.sprite;
        //    //ingredientSlot.gameObject.SetActive(false);
        //    //ingredientSlot.rectTransform.sizeDelta = originalSize;
        //}

        //canCatchArea.SetActive(false);
    }

    public void ShowCraftingResult(Sprite resultSprite)
    {
        canCatchPicture.sprite = resultSprite;
        canCatchArea.SetActive(true);
    }
}

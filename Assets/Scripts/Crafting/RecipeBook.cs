using System.Collections.Generic;
using UnityEngine;

public class MaterialRecipe
{
    public MaterialType MaterialOne { get; }
    public MaterialType MaterialTwo { get; }
    public CombinationType Result { get; }
    public string FishColor { get; }

    public MaterialRecipe(MaterialType materialOne, MaterialType materialTwo, CombinationType result, string fColor)
    {
        MaterialOne = materialOne;
        MaterialTwo = materialTwo;
        Result = result;
        FishColor = fColor;
    }
}

public class ItemDataRecipe
{
    public Material_ItemData MaterialOne { get; }
    public Material_ItemData MaterialTwo { get; }
    public LittleGuy_ItemData Result { get; }
    public string FishColor { get; }

    public ItemDataRecipe(Material_ItemData materialOne, Material_ItemData materialTwo, LittleGuy_ItemData result, string fColor)
    {
        MaterialOne = materialOne;
        MaterialTwo = materialTwo;
        Result = result;
        FishColor = fColor;
    }
}

public static class RecipeBook
{
    private static List<MaterialRecipe> recipes;
    private static List<ItemDataRecipe> itemDataRecipes;
    private static DatabaseWrapper databaseWrapper;

    public static void Initialize(DatabaseWrapper dbWrapper)// holds all of the recipes to be used in the crafting thus far
    {
        databaseWrapper = dbWrapper;
        recipes = new List<MaterialRecipe>
        {
            new MaterialRecipe(MaterialType.Cobweb, MaterialType.Twig, CombinationType.BaitA, "Brown Lure"),
            new MaterialRecipe(MaterialType.Feather, MaterialType.Cobweb, CombinationType.BaitB, "Red Lure"),
            new MaterialRecipe(MaterialType.Flower, MaterialType.Stones, CombinationType.BaitC, "Orange Lure"),
            new MaterialRecipe(MaterialType.Flower, MaterialType.Twig, CombinationType.BaitD, "Yellow Lure"),
            new MaterialRecipe(MaterialType.Stones, MaterialType.Feather, CombinationType.BaitE, "Blue Lure"),
            new MaterialRecipe(MaterialType.Cobweb, MaterialType.Stones, CombinationType.BaitF, "Green Lure"),
            new MaterialRecipe(MaterialType.Feather, MaterialType.Flower, CombinationType.BaitG, "Purple Lure"),
        };

        itemDataRecipes = new List<ItemDataRecipe>();
        foreach (var recipe in recipes)
        {
            Material_ItemData materialOne = databaseWrapper.GetMaterialData(recipe.MaterialOne);
            Material_ItemData materialTwo = databaseWrapper.GetMaterialData(recipe.MaterialTwo);
            LittleGuy_ItemData result = databaseWrapper.GetLittleGuyData(recipe.Result);

            if (materialOne != null && materialTwo != null && result != null)
            {
                itemDataRecipes.Add(new ItemDataRecipe(materialOne, materialTwo, result, result.description));
                Debug.Log($"adding new recipe of {recipe}");
            }
            else
            {
                Debug.LogError($"Failed to convert recipe: {recipe.MaterialOne} + {recipe.MaterialTwo}");
            }
        }
    }
    // check if valid recipe
    public static LittleGuy_ItemData UseRecipe(Material_ItemData first, Material_ItemData second)
    {
        foreach (var recipe in itemDataRecipes)
        {
            if ((recipe.MaterialOne == first && recipe.MaterialTwo == second) ||
                (recipe.MaterialOne == second && recipe.MaterialTwo == first))
            {
                return recipe.Result;
            }
        }

        Debug.Log("Invalid recipe");
        return null;
    }

    public static List<ItemDataRecipe> GetAllRecipes()
    {
        return new List<ItemDataRecipe>(itemDataRecipes);
    }

    public static ItemDataRecipe GetRecipeFor(LittleGuy_ItemData littleGuyData)
    {
        foreach (var recipe in itemDataRecipes)
        {
            if (recipe.Result == littleGuyData)
            {
                return recipe;
            }
        }
        return null;
    }
    public static (Material_ItemData, Material_ItemData)? GetRecipeIngredients(LittleGuy_ItemData littleGuyData)
    {
        foreach (var recipe in itemDataRecipes)
        {
            if (recipe.Result == littleGuyData)
            {
                return (recipe.MaterialOne, recipe.MaterialTwo);
            }
        }
        return null;
    }

    public static bool IsValidRecipe(Material_ItemData one, Material_ItemData two)
    {
        foreach (var recipe in itemDataRecipes)
        {
            if ((recipe.MaterialOne == one && recipe.MaterialTwo == two) ||
                (recipe.MaterialOne == two && recipe.MaterialTwo == one))
            {
                return true;
            }
        }
        return false;
    }
    //public static List<string> GetFormattedValidRecipes() // Get all recipes in a string format
    //{
    //    List<string> validRecipes = new List<string>();

    //    foreach (var recipe in recipes)
    //    {
    //        string formattedRecipe = $"{recipe.MaterialOne} + {recipe.MaterialTwo} = {recipe.FishColor}";
    //        validRecipes.Add(formattedRecipe);
    //    }

    //    return validRecipes;
    //}
}
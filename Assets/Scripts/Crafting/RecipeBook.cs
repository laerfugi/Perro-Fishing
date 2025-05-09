using System.Collections.Generic;
using UnityEngine;

public class MaterialRecipe
{
    public MaterialType MaterialOne { get; }
    public MaterialType MaterialTwo { get; }
    public LittleGuy_ItemData Result { get; }
    public string FishColor { get; }

    public MaterialRecipe(MaterialType materialOne, MaterialType materialTwo, LittleGuy_ItemData result, string fColor)
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
    private static Database database;

    public static void Initialize(Database db)// holds all of the recipes to be used in the crafting thus far
    {
        database = db;
        recipes = new List<MaterialRecipe>
        {
            new MaterialRecipe(MaterialType.Cobweb, MaterialType.Twig, GetLittleGuyData(CombinationType.BaitA), "Brown Lure"),
            new MaterialRecipe(MaterialType.Feather, MaterialType.Cobweb, GetLittleGuyData(CombinationType.BaitB), "Red Lure"),
            new MaterialRecipe(MaterialType.Flower, MaterialType.Stones, GetLittleGuyData(CombinationType.BaitC), "Orange Lure"),
            new MaterialRecipe(MaterialType.Flower, MaterialType.Twig, GetLittleGuyData(CombinationType.BaitD), "Yellow Lure"),
            new MaterialRecipe(MaterialType.Stones, MaterialType.Feather, GetLittleGuyData(CombinationType.BaitE), "Blue Lure"),
            new MaterialRecipe(MaterialType.Cobweb, MaterialType.Stones, GetLittleGuyData(CombinationType.BaitF), "Green Lure"),
            new MaterialRecipe(MaterialType.Feather, MaterialType.Flower, GetLittleGuyData(CombinationType.BaitG), "Purple Lure"),
        };
    }

    private static LittleGuy_ItemData GetLittleGuyData(CombinationType combinationType)
    {
        if (database == null)
        {
            Debug.LogError("Database is not initialized in RecipeBook");
            return null;
        }
        // Iterate through all matching item data
        foreach (var littleGuy in database.littleGuyList)
        {
            if (littleGuy.type == combinationType)
            {
                return littleGuy;
            }
        }

        Debug.LogError($"No LittleGuy_ItemData found for type {combinationType}");
        return null;
    }

    // check if valid recipe
    public static LittleGuy_ItemData UseRecipe(Material_ItemData first, Material_ItemData second)
    {
        foreach (var recipe in recipes)
        {
            if ((recipe.MaterialOne == first.type && recipe.MaterialTwo == second.type) ||
                (recipe.MaterialOne == second.type && recipe.MaterialTwo == first.type))
            {
                return recipe.Result;
            }
        }

        Debug.Log("Invalid recipe");
        return null;
    }

    public static List<MaterialRecipe> GetAllRecipes()
    {
        return new List<MaterialRecipe>(recipes);
    }

    public static MaterialRecipe GetRecipeFor(LittleGuy_ItemData littleGuyData)
    {
        foreach (var recipe in recipes)
        {
            if (recipe.Result == littleGuyData)
            {
                return recipe;
            }
        }
        return null;
    }
    public static (MaterialType, MaterialType)? GetRecipeIngredients(LittleGuy_ItemData littleGuyData)
    {
        foreach (var recipe in recipes)
        {
            if (recipe.Result == littleGuyData)
            {
                return (recipe.MaterialOne, recipe.MaterialTwo);
            }
        }
        return null;
    }

    public static bool IsValidRecipe(MaterialType one, MaterialType two) // Check if both materials create a valid recipe
    {
        foreach (var recipe in recipes)
        {
            if ((recipe.MaterialOne == one && recipe.MaterialTwo == two) ||
                (recipe.MaterialOne == two && recipe.MaterialTwo == one))
            {
                return true;
            }
        }
        return false;
    }
    public static List<string> GetFormattedValidRecipes() // Get all recipes in a string format
    {
        List<string> validRecipes = new List<string>();

        foreach (var recipe in recipes)
        {
            string formattedRecipe = $"{recipe.MaterialOne} + {recipe.MaterialTwo} = {recipe.FishColor}";
            validRecipes.Add(formattedRecipe);
        }

        return validRecipes;
    }
}

/// example usage:
///  public RecipeManager recipeManager;
///  recipeManager.GetCombination(firstMaterial, secondMaterial);
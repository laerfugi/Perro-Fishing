// FOR REFERENCE ONLY
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using System;
//using UnityEngine.SocialPlatforms;

//public class craftUI : MonoBehaviour
//{
//    public GameObject player;
//    public RawImage[] craftIngredients;
//    public List<Texture2D> items;
//    public List<TMP_Text> textCounts;
//    private List<int> scrapedCounts;
//    private List<int> counts;
//    private int currSelected;
//    public SpriteUtility spriteManager;
//    public LittleGuyFactory littleGuyFactory; // shouldnt need to be filled in inspector
//    public GameObject craftingTableUI;
//    public GameObject deployUI;
//    [SerializeField]
//    private Transform littleGuySpawnArea;

//    public Transform inventoryGridView;
//    public GameObject inventoryButtonPrefab;

//    public Transform recipeGridView;
//    public GameObject recipeButtonPrefab;

//    public TMP_Text recipeText;
//    public GameObject recipePanel;
//    public TMP_Text recipeButtonText;

//    public GameObject canCatchArea;
//    public Image canCatchPicture;

//    private SpriteUtility spriteUtility;
//    // private DeploymentUI deploymentUI;
//    private MinigameDeployUI minigameDeployUI;

//    private Vector2 originalSize;

//    private int totalMaterials = System.Enum.GetValues(typeof(MaterialType)).Length;

//    private List<MaterialType> selectedMaterials = new List<MaterialType>(); // store curr materials used

//    private bool openPanel = false;
//    private bool canCraft = true;

//    void Start()
//    {
//        spriteUtility = FindObjectOfType<SpriteUtility>();
//        // deploymentUI = FindObjectOfType<DeploymentUI>();
//        minigameDeployUI = FindObjectOfType<MinigameDeployUI>();
//        originalSize = craftIngredients[0].GetComponent<RawImage>().rectTransform.sizeDelta;
//        ResetCraft();
//        ScrapeFromInventory();
//        SetRecipes();
//        DisplayRecipes();
//        CreateRecipeButtons();
//    }

//    public void ScrapeFromInventory()
//    {
//        //scrapedCounts = new List<int>( new int[MaterialTypeHelper.Count] );
//        counts = new List<int>(new int[MaterialTypeHelper.Count]);
//        for (int i = 0; i < MaterialTypeHelper.Count; i++) // if we plan on adding more mats
//        {
//            counts[i] = Singleton.Instance.mats[i];
//            //scrapedCounts[i] = Singleton.Instance.mats[i];
//            textCounts[i].text = counts[i].ToString();

//            //CreateInventoryEntry(i);
//        }
//    }

//    //void CreateInventoryEntry(int index) // dynamically adds the buttons (somehow doesnt work even though the one for recipe does!!!!)
//    //{
//    //    GameObject newButton = Instantiate(inventoryButtonPrefab, inventoryGridView);
//    //    Image buttonImage = newButton.GetComponentInChildren<Image>();

//    //    buttonImage.sprite = spriteUtility.GetSprite((MaterialType)index);
//    //    buttonImage.preserveAspect = true;

//    //    newButton.GetComponent<Button>().onClick.AddListener(() => InventoryInteract(index));
//    //}
//    //public void InventoryInteract(int index)
//    //{
//    //    MaterialType material = (MaterialType)index;

//    //    if (currSelected <= 1 && counts[index] >= 1)
//    //    {
//    //        textCounts[index].text = (--counts[index]).ToString();
//    //        craftIngredients[currSelected].texture = spriteUtility.GetSprite(material).texture; // modified line, can remove items now
//    //        craftIngredients[currSelected].gameObject.SetActive(true);
//    //        selectedMaterials.Add(material);
//    //        currSelected++;
//    //    }
//    //}

//    public void CloseCraft()
//    {
//        deployUI.SetActive(true);
//        player.GetComponent<PlayerMovement>().enabled = true;
//        Singleton.Instance.isMenuOpened = false;
//        if (!Singleton.Instance.isMenuOpened)
//        {
//            Cursor.lockState = CursorLockMode.Locked;
//            Cursor.visible = false;
//        }

//        //GetComponentInChildren<Canvas>().enabled = false;
//        ResetCraft();
//        craftingTableUI.SetActive(false);
//    }

//    public void ResetCraft()
//    {
//        for (int i = 0; i < craftIngredients.Length; i++)
//        {

//            craftIngredients[i].texture = null;
//            craftIngredients[i].gameObject.SetActive(false);
//            craftIngredients[i].color = new Color(1f, 1f, 1f, 1f); // Back to full opacity
//            //craftIngredients[i].rectTransform.sizeDelta = new Vector2(170, 170); // HARD CODED, based off of craftIngredient1 w and h\
//            craftIngredients[i].rectTransform.sizeDelta = originalSize;
//        }
//        currSelected = 0;
//        selectedMaterials.Clear();
//        canCraft = true;

//        canCatchArea.SetActive(false); // Hide the fish that you can catch

//        //counts = scrapedCounts;
//        ScrapeFromInventory();
//        //for (int i = 0; i < MaterialTypeHelper.Count; i++) // reset text
//        //{
//        //    textCounts[i].text = counts[i].ToString();
//        //}
//    }
//    void CreateRecipeButtons()
//    {
//        List<MaterialRecipe> recipes = RecipeBook.GetAllRecipes();
//        for (int i = 0; i < recipes.Count; i++)
//        {
//            //Debug.Log(recipes[i].FishColor);
//            int index = i;
//            GameObject newButton = Instantiate(recipeButtonPrefab, recipeGridView);
//            newButton.GetComponentInChildren<TMP_Text>().text = $"{recipes[i].FishColor}";
//            Image buttonImage = newButton.GetComponentInChildren<Image>();
            
//            buttonImage.sprite = spriteManager.GetSprite(recipes[i].Result);
//            newButton.GetComponent<Button>().onClick.AddListener(() => AddLittleGuyRecipe(index));
//        }
//    }
//    public void AddLittleGuyRecipe(int index)
//    {
//        //Debug.Log($"index is {index}");
//        ResetCraft();
//        MaterialRecipe currRecipe = RecipeBook.GetAllRecipes()[index];
//        AddMaterial(currRecipe.MaterialOne);
//        AddMaterial(currRecipe.MaterialTwo);
//    }

//    public void CanCatchVisual()
//    {
//        try
//        {
//            CombinationType? fish = RecipeBook.UseRecipe(selectedMaterials[0], selectedMaterials[1]);
//            if (fish != null)
//            {
//                Sprite fishCanCatch = spriteManager.GetFishSprite(fish.Value);
//                canCatchPicture.sprite = fishCanCatch;
//                canCatchArea.SetActive(true);
//            }
//        }
//        catch (ArgumentOutOfRangeException)
//        {
//        }
//    }

//    public void AddMaterial(MaterialType materialType)
//    {
//        int index = (int)materialType;

//        if (currSelected <= 1) // If less than 2 items are selected
//        {
//            RawImage rawImage = craftIngredients[currSelected].GetComponent<RawImage>();
//            rawImage.texture = spriteUtility.GetSprite(materialType).texture; // Apply corresponding sprite
//            rawImage.gameObject.SetActive(true);
//            UIUtility.PreserveAspectRatio(rawImage); // RawImage aspect ratio save

//            if (counts[index] >= 1) // if the user has this material
//            {
//                textCounts[index].text = (--counts[index]).ToString();
//                rawImage.color = new Color(1f, 1f, 1f, 1f); // Full opacity
//                selectedMaterials.Add(materialType);
//            }
//            else // if the user does not have enough material, show ghost of mat, disallow crafting
//            {
//                rawImage.color = new Color(1f, 1f, 1f, 0.2f); // 20% opacity
//                canCraft = false;
//            }
//            //currSelected++;
//            if (++currSelected == 2)
//            {
//                // setup UI on which fish it can catch with this recipe
//                CanCatchVisual();
//            }
//        }
//    }
//    // LMAO
//    public void AddCobweb() => AddMaterial(MaterialType.Cobweb);
//    public void AddFeather() => AddMaterial(MaterialType.Feather);
//    public void AddFlower() => AddMaterial(MaterialType.Flower);
//    public void AddStones() => AddMaterial(MaterialType.Stones);
//    public void AddTwig() => AddMaterial(MaterialType.Twig);


//    public void CraftGuy()
//    {
//        if (currSelected == 2 && canCraft)
//        {

//            MaterialType firstMaterial = selectedMaterials[0];
//            MaterialType secondMaterial = selectedMaterials[1];

//            CombinationType? combination = RecipeBook.UseRecipe(firstMaterial, secondMaterial);

//            // if recipe is not valid
//            if (combination == null) { return; }

//            GameObject createdLittleGuy = LittleGuyFactory.Instance.CreateLittleGuy(littleGuySpawnArea.position, combination.Value);

//            UpdateLittleGuyList(combination.Value, createdLittleGuy);

//            // Remove used materials from inventory
//            Singleton.Instance.mats[(int)firstMaterial]--; 
//            Singleton.Instance.mats[(int)secondMaterial]--;

//            ResetCraft();
//        }
//    }

//    #region deprecated
//    void SetRecipes()
//    {
//        var recipes = RecipeBook.GetFormattedValidRecipes();
//        string recipeList = string.Join("\n", recipes);
//        recipeText.text = recipeList;
//    }

//    public void DisplayRecipes()
//    {
//        openPanel = !openPanel;

//        if (!openPanel)
//        {
//            recipePanel.SetActive(true);
//            recipeButtonText.text = "Close Recipes";
//        }
//        else
//        {
//            recipePanel.SetActive(false);
//            recipeButtonText.text = "Open Recipes";
//        }
//    }
//    #endregion

//    //private CombinationType GetCombination(MaterialType first, MaterialType second) // TODO: fix this
//    //{
//    //    int indexFirst = (int)first;
//    //    int indexSecond = (int)second;

//    //    int combinationIndex = indexFirst + indexSecond;
//    //    int max = System.Enum.GetValues(typeof(CombinationType)).Length - 1;
//    //    combinationIndex = (combinationIndex > max) ? max : combinationIndex;

//    //    return (CombinationType)combinationIndex;
//    //}

//    private void UpdateLittleGuyList(CombinationType combination, GameObject littleGuy)
//    {
//        switch (combination)
//        {
//            case CombinationType.BaitA:
//                Singleton.Instance.BaitALittleGuys.Add(littleGuy);
//                break;
//            case CombinationType.BaitB:
//                Singleton.Instance.BaitBLittleGuys.Add(littleGuy);
//                break;
//            case CombinationType.BaitC:
//                Singleton.Instance.BaitCLittleGuys.Add(littleGuy);
//                break;
//            case CombinationType.BaitD:
//                Singleton.Instance.BaitDLittleGuys.Add(littleGuy);
//                break;
//            case CombinationType.BaitE:
//                Singleton.Instance.BaitELittleGuys.Add(littleGuy);
//                break;
//            case CombinationType.BaitF:
//                Singleton.Instance.BaitFLittleGuys.Add(littleGuy);
//                break;
//            case CombinationType.BaitG:
//                Singleton.Instance.BaitGLittleGuys.Add(littleGuy);
//                break;
//            default:
//                // this should not reach this point
//                Debug.LogWarning("Unknown combination type: " + combination);
//                break;
//        }
//        minigameDeployUI.AddToGrid(spriteUtility.GetSprite(combination), littleGuy);
//        Singleton.Instance.EquipGuy(littleGuy);
//    }
//}

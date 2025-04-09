using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    private Dictionary<MaterialType, Sprite> spriteDictionary;
    private Dictionary<CombinationType, Sprite> playerSpriteDictionary;

    public static SpriteManager Instance { get; private set; }

    [System.Serializable] // why did i need this
    public class MaterialSprite
    {
        public MaterialType type;
        public Sprite sprite;
    }

    [System.Serializable] 
    public class LittleGuySprite
    {
        public CombinationType type;
        public Sprite sprite;
    }

    [System.Serializable]
    public class FishSprite
    {
        public CombinationType type;
        public Sprite sprite;
    }

    public List<MaterialSprite> materialSprites;
    public List<LittleGuySprite> littleGuySprites;
    public List<FishSprite> fishSprites;

    private void Awake()
    {
        spriteDictionary = new Dictionary<MaterialType, Sprite>();
        playerSpriteDictionary = new Dictionary<CombinationType, Sprite>();

        foreach (var materialSprite in materialSprites)
        {
            spriteDictionary[materialSprite.type] = materialSprite.sprite;
        }

        foreach (var lilSprite in littleGuySprites)
        {
            playerSpriteDictionary[lilSprite.type] = lilSprite.sprite;
        }

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Sprite GetSprite(MaterialType type)
    {
        if (spriteDictionary.TryGetValue(type, out Sprite sprite))
        {
            return sprite;
        }
        return null; // where did it go???
    }

    public Sprite GetSprite(CombinationType type)
    {
        if (playerSpriteDictionary.TryGetValue(type, out Sprite sprite))
        {
            return sprite;
        }
        return null;
    }

    public Sprite GetFishSprite(CombinationType type)
    {     
        foreach (var item in fishSprites)
        {
            if (item.type == type) { return item.sprite; }
        }
        return null;
    }
}

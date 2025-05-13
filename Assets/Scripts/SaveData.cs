using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    private const string SaveFileName = "/save.json";

    private void OnEnable()
    {
        EventManager.SaveEvent += Save;
        EventManager.LoadEvent += Load;
    }

    private void OnDisable()
    {
        EventManager.SaveEvent -= Save;
        EventManager.LoadEvent -= Load;
    }

    public void Save()
    {
        PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();

        SaveDataModel saveData = new SaveDataModel
        {
            money = playerInventory.money,
            itemInventoryList = playerInventory.itemInventoryList,
            fishInventoryList = playerInventory.fishInventoryList,
            littleGuyInventoryList = new List<LittleGuy_ItemDataWrapper>()
        };

        foreach (var littleGuyWrapper in playerInventory.littleGuyInventoryList)
        {
            saveData.littleGuyInventoryList.Add(new LittleGuy_ItemDataWrapper(littleGuyWrapper.itemData, littleGuyWrapper.littleGuy));
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(Application.persistentDataPath + SaveFileName, json);
        Debug.Log($"Game saved to {Application.persistentDataPath + SaveFileName}");
    }

    public void Load()
    {
        string path = Application.persistentDataPath + SaveFileName;
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Save file not found at {path}. Starting with default inventory.");
            return;
        }

        string json = File.ReadAllText(path);
        SaveDataModel saveData = JsonUtility.FromJson<SaveDataModel>(json);

        PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();

        Transform spawnPoint = GameObject.Find("Spawn Point")?.transform;

        playerInventory.money = saveData.money;
        playerInventory.itemInventoryList = saveData.itemInventoryList ?? new List<ItemDataWrapper>();
        playerInventory.fishInventoryList = saveData.fishInventoryList ?? new List<ItemDataWrapper>();

        // Clear if they have anything
        playerInventory.littleGuyInventoryList.Clear();

        foreach (var littleGuyWrapper in saveData.littleGuyInventoryList)
        {
            GameObject littleGuy = LittleGuySpawner.Instance.CreateLittleGuy(spawnPoint.position, littleGuyWrapper.itemData as LittleGuy_ItemData);
            playerInventory.littleGuyInventoryList.Add(new LittleGuy_ItemDataWrapper(littleGuyWrapper.itemData, littleGuy));
        }

        Debug.Log("Game loaded from save file.");
    }
}

[System.Serializable]
public class SaveDataModel
{
    public int money;
    public List<ItemDataWrapper> itemInventoryList;
    public List<ItemDataWrapper> fishInventoryList;
    public List<LittleGuy_ItemDataWrapper> littleGuyInventoryList;
}
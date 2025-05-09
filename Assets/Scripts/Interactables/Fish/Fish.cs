using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour, IInteractable
{
    [Header("Item Data")]
    public Fish_ItemData itemData;

    [Header("Where to Spawn")]
    public Database database;
    public Lake lake;

    [Header("Debug")]
    public bool skipMinigame;

    private void OnDestroy()
    {
        if (lake) { lake.fishList.Remove(this.gameObject); }
    }

    //instead of adding fish to inventory, initiates a minigame
    public void Interact()
    {
        if (skipMinigame) 
        {
            itemData = database.fishList[Random.Range(0, database.fishList.Count)];
            PlayerInventory.Instance.AddItem(itemData);
            Destroy(gameObject);
            return;
        }
        StartCoroutine(Minigame());
    }

    public void Use()
    {
        Debug.Log("I am a fish");
    }
    public string GetInteractionPrompt()
    {
        return $"[E] {itemData.name}";
    }

    IEnumerator Minigame()
    {
            yield return MinigameManager.Instance.StartCoroutine(MinigameManager.Instance.LaunchMinigames(1));
            ProcessMinigameResults(MinigameManager.Instance.results);
    }

    void ProcessMinigameResults(List<Result> results)
    {
        if (results.Contains(Result.Lose)) 
        { 
            Debug.Log("you lost... i should despawn and run away");
            Destroy(gameObject);

        }
        else 
        { 
            Debug.Log("you won! i should despawn and go to inventory");

            //choose random fish from list
            itemData = database.fishList[Random.Range(0, database.fishList.Count)];

            PlayerInventory.Instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
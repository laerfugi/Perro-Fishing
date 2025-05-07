using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : ItemInteractable
{
    public new Fish_ItemData itemData { get; set; }

    public List<Fish_ItemData> fish_ItemDataList;
    public Lake lake;

    [Header("Debug")]
    public bool skipMinigame;

    private void OnDestroy()
    {
        if (lake) { lake.fishList.Remove(this.gameObject); }
    }

    //instead of adding fish to inventory, initiates a minigame
    public override void Interact()
    {
        if (skipMinigame) 
        {
            itemData = fish_ItemDataList[Random.Range(0, fish_ItemDataList.Count)];
            PlayerInventory.Instance.AddItem(itemData);
            Destroy(gameObject);
            return;
        }
        StartCoroutine(Minigame());
    }

    public override void Use()
    {
        Debug.Log("I am a fish");
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
            itemData = fish_ItemDataList[Random.Range(0, fish_ItemDataList.Count)];

            PlayerInventory.Instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
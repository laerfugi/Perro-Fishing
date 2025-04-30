using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : ItemInteractable
{
    //instead of adding fish to inventory, initiates a minigame
    public override void Interact()
    {
        StartCoroutine(Minigame());
    }

    public override void Use()
    {
        Debug.Log("I am a fish");
    }

    IEnumerator Minigame()
    {
        yield return MinigameManager.Instance.StartCoroutine(MinigameManager.Instance.LaunchMinigame(""));
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
            PlayerInventory.Instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
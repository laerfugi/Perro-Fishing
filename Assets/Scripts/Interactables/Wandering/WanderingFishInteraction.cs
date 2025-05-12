using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WanderingFishInteraction : MonoBehaviour, IInteractable
{
    [Header("Item Data")]
    public Fish_ItemData fishData;

    [Header("Dependencies")]
    public WanderingContainer wanderingContainer;
    public WanderingFishSpawner spawner;

    [Header("Info")]
    public Database database;
    public SpriteRenderer fishSprite;
        
    [Header("Animator")]
    public Animator animator; // Thingy original fish had

    [Header("Debug")]
    public bool skipMinigame;

    private bool isInteracting = false;

    public void Interact()
    {
        if (isInteracting) return;
        isInteracting = true;

        wanderingContainer.isPlayerInteracting = true;
        wanderingContainer.StopWandering();

        Debug.Log("Player interacted with wandering fish.");

        // Start minigame
        if (skipMinigame)
        {
            List<Result> temp = new List<Result>();
            temp.Add(Result.Win);
            ProcessMinigameResults(temp);
        }
        else
        {
            StartCoroutine(Minigame());
        }
    }

    public string GetInteractionPrompt()
    {
        return $"[E] Catch fish"; // {fishData.name}
    }
    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.RemoveNPC(gameObject);
        }
    }

    public void SetSprite(Sprite sprite)
    {
        if (fishSprite != null)
        {
            fishSprite.sprite = sprite;
        }
    }

    private IEnumerator Minigame()
    {
        this.gameObject.layer = 0;

        Debug.Log("Starting minigame for wandering fish.");

        yield return MinigameManager.Instance.StartCoroutine(MinigameManager.Instance.LaunchMinigames(1));

        ProcessMinigameResults(MinigameManager.Instance.results);
    }

    private void ProcessMinigameResults(List<Result> results)
    {
        if (results.Contains(Result.Win))
        {
            //Fish_ItemData caughtFish = database.fishList[Random.Range(0, database.fishList.Count)];

            PlayerInventory.Instance.AddItem(fishData);
        }
        //StartCoroutine(Animate());
        Destroy(gameObject);
    }

    IEnumerator Animate()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 90);
        animator.SetBool("Catch", true);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Animation finished"));
        Destroy(gameObject);
    }
}
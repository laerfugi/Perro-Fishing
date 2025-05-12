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
    public float rateUp;
        
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

        //Debug.Log("Player interacted with wandering fish.");

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

        //Debug.Log("Starting minigame for wandering fish.");

        yield return MinigameManager.Instance.StartCoroutine(MinigameManager.Instance.LaunchMinigames(1));

        ProcessMinigameResults(MinigameManager.Instance.results);
    }

    private void ProcessMinigameResults(List<Result> results)
    {
        if (results.Contains(Result.Win))
        {
            //Fish_ItemData caughtFish = database.fishList[Random.Range(0, database.fishList.Count)];
            // Find fishing pole
            FishingPole fishingPole = GameObject.FindWithTag("Player").GetComponentInChildren<FishingPole>();
            if (fishingPole == null || fishingPole.baitLittleGuy_ItemDataWrapper == null)
            {
                Debug.LogError("FishingPole or baitLittleGuy_ItemDataWrapper not found!");
                return;
            }
            // Get little guy data
            LittleGuy_ItemData littleGuyData = fishingPole.baitLittleGuy_ItemDataWrapper.itemData as LittleGuy_ItemData;
            if (littleGuyData == null)
            {
                Debug.LogError("LittleGuy_ItemData not found!");
                return;
            }

            // Roll for a random fish
            CombinationType baitType = littleGuyData.type;

            Fish_ItemData caughtFish;
            float roll = Random.Range(0f, 1f);

            if (roll <= rateUp)
            {
                // Get a fish with the same CombinationType
                List<Fish_ItemData> matchingFish = database.fishList.FindAll(fish => fish.type == baitType);
                if (matchingFish.Count > 0)
                {
                    Debug.Log("rate up win!");
                    caughtFish = matchingFish[0];
                }
                else
                {
                    Debug.LogWarning("you messed up the rolling for rate up fish! (rate up win)");
                    caughtFish = database.fishList[Random.Range(0, database.fishList.Count)];
                }
            }
            else
            {
                // Get a random fish that is not of the same CombinationType
                List<Fish_ItemData> nonMatchingFish = database.fishList.FindAll(fish => fish.type != baitType);
                if (nonMatchingFish.Count > 0)
                {
                    Debug.Log("rate up lost!");
                    caughtFish = nonMatchingFish[Random.Range(0, nonMatchingFish.Count)];
                }
                else
                {
                    Debug.LogWarning("you messed up the rolling for rate up fish! (rate up loss)");
                    caughtFish = database.fishList[Random.Range(0, database.fishList.Count)];
                }
            }
            PlayerInventory.Instance.AddItem(caughtFish);
        }
        //StartCoroutine(Animate());
        Destroy(gameObject);
    }

    // Unused for now
    IEnumerator Animate()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 90);
        animator.SetBool("Catch", true);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Animation finished"));
        Destroy(gameObject);
    }
}
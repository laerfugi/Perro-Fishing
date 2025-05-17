using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [Header("Quest Settings")]
    public int maxQuests;
    public int minFishAmount = 1;
    public int maxFishAmount = 2;

    private DatabaseWrapper dbWrapper;

    private int totalClaimed = 0;
    private List<Quest> activeQuests = new List<Quest>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        dbWrapper = GetComponent<DatabaseWrapper>();
        GenerateQuests();
    }

    public void GenerateQuests()
    {
        activeQuests.Clear();

        for (int i = 0; i < maxQuests; i++)
        {
            activeQuests.Add(GenerateRandomQuest());
        }

        QuestBoardUI.Instance.UpdateQuestBoard(activeQuests);
    }

    private Quest GenerateRandomQuest()
    {
        Fish_ItemData randomFish = dbWrapper.database.fishList[Random.Range(0, dbWrapper.database.fishList.Count)];
        int randomAmount = Random.Range(minFishAmount, maxFishAmount + 1);

        return new Quest
        {
            name = $"Catch {randomAmount} {randomFish.name}(s)",
            fishRequest = randomFish,
            amount = randomAmount,
            reward = randomAmount * randomFish.price,
            complete = false,
            claimed = false
        };
    }

    // Check if the player has the materials
    public void ValidateQuest(Quest quest)
    {
        int playerFishCount = PlayerInventory.Instance.InInventory(quest.fishRequest)?.count ?? 0;
        //if (quest.claimed)
        //{
        //    return;
        //}
        quest.complete = playerFishCount >= quest.amount;
    }

    public void ClaimReward(Quest quest)
    {
        Debug.Log($"QUEST {quest.name} IS BEING CLAIMED");
        if (quest.complete && !quest.claimed)
        {
            AudioManager.Instance.PlaySound("MoneyGained");
            for (int i = 0; i < quest.amount; i++)
            {
                PlayerInventory.Instance.RemoveItem(quest.fishRequest);
            }
            PlayerInventory.Instance.AddMoney(quest.reward);

            totalClaimed += 1;
            quest.claimed = true;

            ValidateAllQuests();
            QuestBoardUI.Instance.UpdateQuestBoard(activeQuests);
        }

        // Regen quests
        if (totalClaimed >= maxQuests)
        {
            totalClaimed = 0;
            GenerateQuests();
        }
    }

    public void ValidateAllQuests()
    {
        foreach (Quest quest in activeQuests)
        {
            ValidateQuest(quest);
        }
    }

    public List<Quest> GetActiveQuests()
    {
        return activeQuests;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestBoardUI : MenuClass
{
    public static QuestBoardUI Instance { get; private set; }

    [Header("UI Elements")]
    public Transform questContainer;
    public GameObject questUIPrefab;

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
    public void UpdateQuestBoard(List<Quest> quests)
    {
        // Clear existing quests
        foreach (Transform child in questContainer)
        {
            Destroy(child.gameObject);
        }

        // Add new quests
        foreach (Quest quest in quests)
        {
            GameObject questUIObject = Instantiate(questUIPrefab, questContainer);
            QuestUI questUI = questUIObject.GetComponent<QuestUI>();
            questUI.Initialize(quest);
        }
    }

    public void ToggleUI()
    {
        ToggleMenu();
        QuestManager.Instance.ValidateAllQuests();
        UpdateQuestBoard(QuestManager.Instance.GetActiveQuests());
    }
}

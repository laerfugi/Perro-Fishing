using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text questNameText;
    //public TMP_Text fishTypeText;
    //public TMP_Text amountText;
    public TMP_Text rewardText;
    public Image fishIcon;
    public Button claimButton;

    private Quest quest;

    public void Initialize(Quest quest)
    {
        this.quest = quest;

        questNameText.text = quest.name;
        //fishTypeText.text = quest.fishRequest.name;
        //amountText.text = $"Amount: {quest.amount}";
        rewardText.text = $"${quest.reward}";
        fishIcon.sprite = quest.fishRequest.icon;

        UpdateClaimButton();
    }

    public void UpdateClaimButton()
    {
        claimButton.interactable = quest.complete && !quest.claimed;
        claimButton.image.color = quest.complete ? Color.green : Color.gray;
    }

    public void OnClaimButtonClicked()
    {
        QuestManager.Instance.ClaimReward(quest);
    }
}
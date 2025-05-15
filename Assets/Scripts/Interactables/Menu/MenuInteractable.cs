using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInteractable : MonoBehaviour, IInteractable
{
    [Header("Menu Reference")]
    [SerializeField] private MenuClass menu;
    [SerializeField] private string functionToCall = "ToggleMenu";

    [Header("Interaction Prompt")]
    [SerializeField] private string interactionPrompt = "[E] Open crafting table";
    public void Interact()
    {
        if (menu != null)
        {
            menu.SendMessage(functionToCall, SendMessageOptions.DontRequireReceiver); // no error
        }
        else
        {
            Debug.LogWarning("Menu reference is not assigned in MenuInteractable");
        }
    }

    public string GetInteractionPrompt()
    {
        return interactionPrompt;
    }
}
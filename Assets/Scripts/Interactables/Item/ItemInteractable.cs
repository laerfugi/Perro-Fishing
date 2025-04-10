using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName;

    public void Interact()
    {
        Debug.Log($"Picked up {itemName}");
        // add to inventory thingy
        Destroy(gameObject);
    }

    public string GetInteractionPrompt()
    {
        return $"Press E to pick up {itemName}";
    }
}
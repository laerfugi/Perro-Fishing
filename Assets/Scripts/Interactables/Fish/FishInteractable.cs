using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Fish_ItemData fishData;

    public void Interact()
    {
        Debug.Log($"Attempting to pick up {fishData.name} of {fishData.combinationType}");
        if (!PlayerInventory.Instance.IsFull())
        {
            PlayerInventory.Instance.AddItem(fishData);
            Destroy(gameObject);
        }
    }

    public string GetInteractionPrompt()
    {
        return $"Press E to pick up {fishData.name}";
    }
}
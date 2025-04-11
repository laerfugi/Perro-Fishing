using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//parent class for all items in the game that can go in the player inventory and be held (fish, flashlight)
public abstract class ItemInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] protected ItemData itemData;

    public void Interact()
    {
        Debug.Log($"Attempting to pick up {itemData.name}");
        if (!PlayerInventory.Instance.IsFull())
        {
            PlayerInventory.Instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }

    public string GetInteractionPrompt()
    {
        return $"Press E to pick up {itemData.name}";
    }

    public virtual void Use() { }

}
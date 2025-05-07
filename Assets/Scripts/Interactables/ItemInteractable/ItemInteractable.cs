using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//parent class for all worldspace representations of items in the game that can go in the player inventory and be held (fish, flashlight)
public abstract class ItemInteractable : MonoBehaviour, IInteractable
{
    [field: SerializeField]
    public ItemData itemData { get; set; }

    public virtual void Interact()
    {
        Debug.Log($"Attempting to pick up {itemData.name}");
        //if (!PlayerInventory.Instance.IsFull())
        //{
            PlayerInventory.Instance.AddItem(itemData);
            Destroy(gameObject);
        //}
    }

    public string GetInteractionPrompt()
    {
        return $"[E] {itemData.name}";
    }

    public virtual void Use() { }

}
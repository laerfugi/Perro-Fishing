using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractHitbox : MonoBehaviour
{
    [SerializeReference]
    public List<GameObject> interactablesInHitbox = new List<GameObject>();

    void OnTriggerEnter(Collider collider)
    {
        IInteractable interactable = collider.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactablesInHitbox.Add(collider.gameObject);

            if (interactablesInHitbox.Count == 1)
            {
                EventManager.OnPlayerCanInteractEvent(interactable.GetInteractionPrompt());
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        IInteractable interactable = collider.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactablesInHitbox.Remove(collider.gameObject);

            if (interactablesInHitbox.Count == 0)
            {
                EventManager.OnPlayerCannotInteractEvent();
            }
        }
    }

    public void InteractWithClosest()
    {
        if (interactablesInHitbox.Count > 0)
        {
            IInteractable closestInteractable = interactablesInHitbox[0].GetComponent<IInteractable>();
            closestInteractable.Interact();

            interactablesInHitbox.Remove(interactablesInHitbox[0]);

            if (interactablesInHitbox.Count == 0)
            {
                EventManager.OnPlayerCannotInteractEvent();
            }
            
        }
    }
}
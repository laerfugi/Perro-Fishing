using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractHitbox : MonoBehaviour
{
    public List<IInteractable> interactablesInHitbox = new List<IInteractable>();

    void OnTriggerEnter(Collider collider)
    {
        IInteractable interactable = collider.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactablesInHitbox.Add(interactable);

            if (interactablesInHitbox.Count == 1)
            {
                EventManager.OnPlayerCanInteractEvent();
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        IInteractable interactable = collider.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactablesInHitbox.Remove(interactable);

            if (interactablesInHitbox.Count == 0)
            {
                EventManager.OnPlayerCanInteractEvent();
            }
        }
    }

    public void InteractWithClosest()
    {
        if (interactablesInHitbox.Count > 0)
        {
            IInteractable closestInteractable = interactablesInHitbox[0];
            closestInteractable.Interact();

            interactablesInHitbox.Remove(closestInteractable);

            if (interactablesInHitbox.Count == 0)
            {
                EventManager.OnPlayerCannotInteractEvent();
            }
        }
    }
}
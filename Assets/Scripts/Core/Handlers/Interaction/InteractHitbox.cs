using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Contains a list of interactable gameobjects (objects with an IInteractable component).
//Interactable gameobjects should have a PlayerInteractable or LittleGuyInteractable layer

public class InteractHitbox : MonoBehaviour
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
                EventManager.OnCanInteractEvent(interactable.GetInteractionPrompt());
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
                EventManager.OnCannotInteractEvent();
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
                EventManager.OnCannotInteractEvent();
            }
            
        }
    }
}
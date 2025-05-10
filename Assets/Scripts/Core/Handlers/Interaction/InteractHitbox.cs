using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Contains a list of interactable gameobjects (objects with an IInteractable component).
//Interactable gameobjects should have a PlayerInteractable or LittleGuyInteractable layer

public class InteractHitbox : MonoBehaviour
{
    public List<GameObject> interactablesInHitbox;
    [SerializeReference]
    public List<IInteractable> IInteractables = new List<IInteractable>();


    //remove null references
    private void Update()
    {
        CheckNull();

        if (interactablesInHitbox.Count >= 1)
        {
            EventManager.OnCanInteractEvent(IInteractables[0].GetInteractionPrompt());
        }
        else
        {
            EventManager.OnCannotInteractEvent();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        IInteractable interactable = collider.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactablesInHitbox.Add(collider.gameObject);
            IInteractables.Add(interactable);
            Debug.Log("thing added");
        }
    }

    void OnTriggerExit(Collider collider)
    {
        IInteractable interactable = collider.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactablesInHitbox.Remove(collider.gameObject);
            IInteractables.Remove(interactable);
        }
    }

    public void InteractWithClosest()
    {
        CheckNull();
        if (interactablesInHitbox.Count > 0)
        {
            EventManager.OnCannotInteractEvent();

            IInteractable closestInteractable = IInteractables[0];
            closestInteractable.Interact();
        }
    }

    //scuffed but it works
    void CheckNull()
    {
        if (interactablesInHitbox.Count > 0 && interactablesInHitbox[0] == null) { interactablesInHitbox.Remove(interactablesInHitbox[0]); IInteractables.Remove(IInteractables[0]); }
    }
}
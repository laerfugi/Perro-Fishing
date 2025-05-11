using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Contains a list of interactable gameobjects (objects with an IInteractable component).
//Interactable gameobjects should have a PlayerInteractable or LittleGuyInteractable layer

public class InteractHitbox : MonoBehaviour
{
    public List<GameObject> interactablesInHitbox = new List<GameObject>();

    private void OnEnable()
    {
        interactablesInHitbox.Clear();
    }

    //remove null references
    private void Update()
    {
        CheckNull();
    }

    void OnTriggerEnter(Collider collider)
    {
        IInteractable interactable = collider.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactablesInHitbox.Add(collider.gameObject);

            if (interactablesInHitbox.Count >= 1)
            {
                EventManager.OnCanInteractEvent(interactable.GetInteractionPrompt());
                Debug.Log("First thing called");
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        IInteractable interactable = collider.GetComponent<IInteractable>();
        if (interactable != null)
        {
            if (interactablesInHitbox.Count > 1) { EventManager.OnCanInteractEvent(interactablesInHitbox[1].GetComponent<IInteractable>().GetInteractionPrompt()); }    //update ui with next interactable in list

            interactablesInHitbox.Remove(collider.gameObject);
            EventManager.OnCannotInteractEvent();
        }
    }

    public void InteractWithClosest()
    {
        CheckNull();
        if (interactablesInHitbox.Count > 0)
        {
            if (interactablesInHitbox.Count >1) { EventManager.OnCanInteractEvent(interactablesInHitbox[1].GetComponent<IInteractable>().GetInteractionPrompt()); }    //update ui with next interactable in list


            IInteractable closestInteractable = interactablesInHitbox[0].GetComponent<IInteractable>();
            closestInteractable.Interact();
            Debug.Log("thing called");
            EventManager.OnCannotInteractEvent();
        }
    }

    //scuffed but it works
    void CheckNull()
    {
        if (interactablesInHitbox.Count > 0 && interactablesInHitbox[0] == null) { interactablesInHitbox.Remove(interactablesInHitbox[0]); EventManager.OnCannotInteractEvent(); }
        //if (interactablesInHitbox.Count == 0) { EventManager.OnCannotInteractEvent(); }     //clears notif in case if it hasn't been cleared
    }
}
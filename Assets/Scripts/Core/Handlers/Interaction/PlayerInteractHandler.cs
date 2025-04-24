using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractHandler : MonoBehaviour, IInteractHandler
{
    public InteractHitbox interactHitbox { get; private set; }

    void Start()
    {
        interactHitbox = GetComponentInChildren<InteractHitbox>();
    }

    public void HandleInteract(IInputHandler inputHandler)
    {
        if (inputHandler.IsPressingInteract)
        {
            interactHitbox.InteractWithClosest();
        }
    }
}
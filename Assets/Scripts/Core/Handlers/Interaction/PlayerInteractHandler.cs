using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractHandler : MonoBehaviour, IInteractHandler
{
    public PlayerInteractHitbox interactHitbox { get; private set; }

    void Start()
    {
        interactHitbox = GetComponentInChildren<PlayerInteractHitbox>();
    }

    public void HandleInteract(IInputHandler inputHandler)
    {
        if (inputHandler.IsPressingInteract)
        {
            interactHitbox.InteractWithClosest();
        }
    }
}
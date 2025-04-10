using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInteractHitbox interactHitbox;

    void Start()
    {
        interactHitbox = GetComponentInChildren<PlayerInteractHitbox>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            interactHitbox.InteractWithClosest();
        }
    }
}
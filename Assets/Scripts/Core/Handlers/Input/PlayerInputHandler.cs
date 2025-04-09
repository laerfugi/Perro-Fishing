using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour, IInputHandler
{
    public float Vertical { get; private set; }
    public float Horizontal { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsSprinting { get; private set; }
    public bool IsClicking { get; private set; }
    public bool IsPressingInteract { get; private set; }
    public bool IsPressingDrop { get; private set; }
    public bool IsPressingMenu { get; private set; }

    public void HandleInput()
    {
        // Movement inputs
        Vertical = Input.GetAxisRaw("Vertical");
        Horizontal = Input.GetAxisRaw("Horizontal");

        // Action inputs
        IsJumping = Input.GetKeyDown(KeyCode.Space);
        IsSprinting = Input.GetKey(KeyCode.LeftShift);

        // Interaction inputs
        IsClicking = Input.GetMouseButtonDown(0); // Left mouse button
        IsPressingInteract = Input.GetKeyDown(KeyCode.E);
        IsPressingDrop = Input.GetKeyDown(KeyCode.R);
        IsPressingMenu = Input.GetKeyDown(KeyCode.Tab);
    }
}
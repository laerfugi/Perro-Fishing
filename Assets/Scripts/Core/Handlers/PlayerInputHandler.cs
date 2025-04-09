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
    public bool IsPressingE { get; private set; }
    public bool IsPressingR { get; private set; }
    public bool IsPressingTab { get; private set; }

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
        IsPressingE = Input.GetKeyDown(KeyCode.E);
        IsPressingR = Input.GetKeyDown(KeyCode.R);
        IsPressingTab = Input.GetKeyDown(KeyCode.Tab);
    }
}
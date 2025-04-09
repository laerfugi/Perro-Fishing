using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour, IInputHandler
{
    public float Vertical { get; private set; }
    public float Horizontal { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsSprinting { get; private set; }

    public void HandleInput()
    {
        Vertical = Input.GetAxisRaw("Vertical");
        Horizontal = Input.GetAxisRaw("Horizontal");
        IsJumping = Input.GetKeyDown(KeyCode.Space);
        IsSprinting = Input.GetKey(KeyCode.LeftShift);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputHandler
{
    float Vertical { get; }
    float Horizontal { get; }
    bool IsJumping { get; }
    bool IsSprinting { get; }
    bool IsClicking { get; }
    bool IsPressingInteract { get; }
    bool IsPressingDrop { get; }
    bool IsPressingMenu { get; }

    void HandleInput();
}
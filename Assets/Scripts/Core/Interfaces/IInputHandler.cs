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
    bool IsPressingE { get; }
    bool IsPressingR { get; }
    bool IsPressingTab { get; }

    void HandleInput();
}
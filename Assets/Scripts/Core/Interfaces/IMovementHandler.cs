using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementHandler
{
    bool IsGrounded { get; }
    bool IsMoving { get; }
    bool IsSprinting { get; }

    void HandleMovement(IInputHandler inputHandler);
}
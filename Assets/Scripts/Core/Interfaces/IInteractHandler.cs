using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractHandler
{
    InteractHitbox interactHitbox { get;}

    void HandleInteract(IInputHandler inputHandler);
}

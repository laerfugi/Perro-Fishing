using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractHandler
{
    PlayerInteractHitbox interactHitbox { get;}

    void HandleInteract(IInputHandler inputHandler);
}

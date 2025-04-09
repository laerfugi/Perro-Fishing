using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private IInputHandler inputHandler;
    private IMovementHandler movementHandler;
    void Start()
    {
        inputHandler = GetComponent<IInputHandler>();
        movementHandler = GetComponent<IMovementHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        inputHandler.HandleInput();
        movementHandler.HandleMovement(inputHandler);
    }
}

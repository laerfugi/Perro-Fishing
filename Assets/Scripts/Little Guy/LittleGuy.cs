using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LittleGuyNav))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NavMeshAgent))]
public class LittleGuy : MonoBehaviour
{
    private LittleGuyNav navHandler;
    private IMovementHandler movementHandler;
    private IInputHandler inputHandler;

    private NavMeshAgent nav;
    private CharacterController controller;

    private bool isPlayerControlled;

    void Start()
    {
        // Get handlers
        navHandler = GetComponent<LittleGuyNav>();
        movementHandler = GetComponent<IMovementHandler>();
        inputHandler = GetComponent<IInputHandler>();

        // Get components to enable/disable
        nav = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();

        SetAIControlled();
    }

    void Update()
    {
        if (isPlayerControlled)
        {
            inputHandler.HandleInput();
            movementHandler.HandleMovement(inputHandler);
        }
        else
        {
            navHandler.HandleAI();
        }
    }

    public void SetPlayerControlled()
    {
        isPlayerControlled = true;
        nav.enabled = false;
        controller.enabled = true;
    }

    public void SetAIControlled()
    {
        isPlayerControlled = false;

        if (!IsGrounded())
        {
            ForceGrounded();
        }

        controller.enabled = false;
        nav.enabled = true;
    }

    private bool IsGrounded()
    {
        return controller.isGrounded;
    }

    private void ForceGrounded()
    {
        // Force the LittleGuy to snap to the ground
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
    }
}
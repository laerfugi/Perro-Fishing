using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum LittleGuyState { AI, Active, Inactive}

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
    public GameObject vCam;     //idk how to make this private
    private CameraPivot cameraPivot;

    //private bool isPlayerControlled;
    [Header("State")]
    public LittleGuyState state;

    void Start()
    {
        // Get handlers
        navHandler = GetComponent<LittleGuyNav>();
        movementHandler = GetComponent<IMovementHandler>();
        inputHandler = GetComponent<IInputHandler>();

        // Get components to enable/disable
        nav = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
        cameraPivot = GetComponentInChildren<CameraPivot>();

        //SetAIControlled();
    }

    void Update()
    {
        if (state == LittleGuyState.AI)                     //Little Guy is AI controlled
        {
            //state stuff
            controller.enabled = false;
            nav.enabled = true;

            navHandler.HandleAI();
        }
        else if (state == LittleGuyState.Active)            //Little Guy is player controlled
        {
            //camera stuff
            EventManager.OnSwitchVCamEvent(vCam);
            cameraPivot.enabled = true;

            //state stuff
            nav.enabled = false;
            controller.enabled = true;

            inputHandler.HandleInput();
            movementHandler.HandleMovement(inputHandler);
        }
        else if (state == LittleGuyState.Inactive)          //Little Guy can't move
        {
            //camera stuff
            cameraPivot.enabled = false;
        }
    }

    /*
    public void SetPlayerControlled()
    {
        //isPlayerControlled = true;
        state = LittleGuyState.Active;
        nav.enabled = false;
        controller.enabled = true;
    }

    public void SetAIControlled()
    {
        //isPlayerControlled = false;
        state = LittleGuyState.AI;

        if (!IsGrounded())
        {
            ForceGrounded();
        }

        controller.enabled = false;
        nav.enabled = true;
    }
    */

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
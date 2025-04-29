using System;
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
    private IInteractHandler interactHandler;

    private NavMeshAgent nav;
    private CharacterController controller;
    public GameObject vCam;     //idk how to make this private
    private CameraPivot cameraPivot;

    [field: Header("State")]
    [field: SerializeField]
    public LittleGuyState state { get; private set; }

    private LittleGuyState previousState;  //for MenuEventCheck()

    //Events
    private void OnEnable()
    {
        EventManager.OpenMenuEvent += OpenMenuEventCheck;   //if menu is toggled, change player state
        EventManager.CloseMenuEvent +=  CloseMenuEventCheck;
    }

    private void OnDisable()
    {
        EventManager.OpenMenuEvent -= OpenMenuEventCheck;
        EventManager.CloseMenuEvent -= CloseMenuEventCheck;
    }

    void Start()
    {
        // Get handlers
        navHandler = GetComponent<LittleGuyNav>();
        movementHandler = GetComponent<IMovementHandler>();
        inputHandler = GetComponent<IInputHandler>();
        interactHandler = GetComponent<IInteractHandler>();

        // Get components to enable/disable
        nav = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
        cameraPivot = GetComponentInChildren<CameraPivot>();

        //Change State to AI
        ChangeState(LittleGuyState.AI);
    }

    void Update()
    {
        if (state == LittleGuyState.AI)                     //Little Guy is AI controlled
        {
            if (!IsGrounded())
            {
                //ForceGrounded();
            }

            navHandler.HandleAI();
        }
        else if (state == LittleGuyState.Active)            //Little Guy is player controlled
        {
            inputHandler.HandleInput();
            movementHandler.HandleMovement(inputHandler);
            interactHandler.HandleInteract(inputHandler);
        }
        else if (state == LittleGuyState.Inactive)          //Little Guy can't move
        {

        }
    }

    /*---State Change methods---*/
    #region State Change Methods
    public void ChangeState(LittleGuyState littleGuyState)
    {
        state = littleGuyState;

        if (state == LittleGuyState.AI)                     //Little Guy is AI controlled
        {
            //state stuff
            controller.enabled = false;
            nav.enabled = true;
        }
        else if (state == LittleGuyState.Active)            //Little Guy is player controlled
        {
            //camera stuff
            EventManager.OnSwitchVCamEvent(vCam);

            //state stuff
            nav.enabled = false;
            controller.enabled = true;
        }
        else if (state == LittleGuyState.Inactive)          //Little Guy can't move
        {
            // turn off nav early to fix position snapping bug
            nav.enabled = false;
        }

        //EventManager.OnPlayerStateEvent(PlayerState.Active);
        EventManager.OnLittleGuyStateEvent(state);
    }

    //used by menu events
    void OpenMenuEventCheck()
    {
        if (state == LittleGuyState.Active) { previousState = state; ChangeState(LittleGuyState.Inactive); }
    }

    void CloseMenuEventCheck()
    {
        if (state == LittleGuyState.Inactive) { ChangeState(previousState); }
    }

    #endregion

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
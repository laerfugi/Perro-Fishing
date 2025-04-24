using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum LittleGuyState { AI, Active, Inactive, Menu}

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
        EventManager.MenuEvent += () => MenuEventCheck();
    }

    private void OnDisable()
    {
        EventManager.MenuEvent -= () => MenuEventCheck();
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
        else if (state == LittleGuyState.Menu)          //Little Guy and cam can't move
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
            //camera stuff
            cameraPivot.enabled = true;

            //state stuff
            controller.enabled = false;
            nav.enabled = true;
        }
        else if (state == LittleGuyState.Active)            //Little Guy is player controlled
        {
            //camera stuff
            EventManager.OnSwitchVCamEvent(vCam);
            cameraPivot.enabled = true;

            //state stuff
            nav.enabled = false;
            controller.enabled = true;
        }
        else if (state == LittleGuyState.Inactive)          //Little Guy can't move
        {
            //camera stuff
            cameraPivot.enabled = true;

            // turn off nav early to fix position snapping bug
            nav.enabled = false;
        }
        else if (state == LittleGuyState.Menu)          //Little Guy and cam can't move
        {
            //camera stuff
            cameraPivot.enabled = false;

            // turn off nav early to fix position snapping bug
            nav.enabled = false;
        }

        //EventManager.OnPlayerStateEvent(PlayerState.Active);
        EventManager.OnLittleGuyStateEvent(state);
    }

    //used by menu event
    void MenuEventCheck()
    {
        if (state == LittleGuyState.Active || state == LittleGuyState.AI) { previousState = state;  ChangeState(LittleGuyState.Menu); }
        else if (state == LittleGuyState.Menu) { ChangeState(previousState); }
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
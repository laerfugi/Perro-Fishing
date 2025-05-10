using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum LittleGuyState { AI, Active, Inactive,InMenu}

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
    private SpriteRenderer spriteRenderer;
    public GameObject interactHitbox;

    [field: Header("State")]
    [field: SerializeField]
    public LittleGuyState state { get; private set; }
    private LittleGuyState previousState;  //for MenuEventCheck()

    [Header("ItemData")]
    public LittleGuy_ItemData itemData;

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

    private void OnDestroy()
    {
        //Remove from player inventory
        PlayerInventory.Instance.RemoveLittleGuy(this);
    }

    void Start()
    {
        // Get handlers
        navHandler = GetComponent<LittleGuyNav>();
        movementHandler = GetComponent<IMovementHandler>();
        inputHandler = GetComponent<IInputHandler>();
        interactHandler = GetComponent<IInteractHandler>();

        // Get components
        nav = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
        cameraPivot = GetComponentInChildren<CameraPivot>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = itemData.icon;

        //Change State to AI
        ChangeState(LittleGuyState.AI);

        //Add to player inventory
        PlayerInventory.Instance.AddLittleGuy(this);
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
        else if (state == LittleGuyState.InMenu)          
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
            SnapToNav();
            //state stuff
            //controller.enabled = false;
            //nav.enabled = true;

            interactHitbox.SetActive(false);
        }
        else if (state == LittleGuyState.Active)            //Little Guy is player controlled
        {
            //camera stuff
            EventManager.OnSwitchVCamEvent(vCam);

            //state stuff
            nav.enabled = false;
            controller.enabled = true;

            interactHitbox.SetActive(true);
        }
        else if (state == LittleGuyState.Inactive)          //Little Guy can't move
        {
            // turn off nav early to fix position snapping bug
            nav.enabled = false;

            interactHitbox.SetActive(false);
        }

        //EventManager.OnPlayerStateEvent(PlayerState.Active);
        EventManager.OnLittleGuyStateEvent(state);
    }

    //used by menu events
    void OpenMenuEventCheck()
    {
        if (state == LittleGuyState.Active) { previousState = state; ChangeState(LittleGuyState.InMenu); }
    }

    void CloseMenuEventCheck()
    {
        if (state == LittleGuyState.InMenu) { ChangeState(previousState); }
    }

    #endregion

    private void SnapToNav()
    {
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit temp, 0.5f, NavMesh.AllAreas))
        {
            // Already on surface
            controller.enabled = false;
            nav.enabled = true;
            return;
        }

        StartCoroutine(SnapAfterDelay());
    }

    private IEnumerator SnapAfterDelay()
    {
        yield return new WaitForSeconds(2f); // Equal to the camera blend setting
        if (state == LittleGuyState.Active) yield break;

        float maxDistance = 30f;
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
        {
            transform.position = hit.position + new Vector3(0, 0.5f, 0);
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
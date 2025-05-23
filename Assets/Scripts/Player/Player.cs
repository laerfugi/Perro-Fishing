using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {Active,Inactive,InMenu,Fishing}

public class Player : MonoBehaviour
{
    private IInputHandler inputHandler;
    private IMovementHandler movementHandler;
    private IInteractHandler interactHandler;

    public GameObject vCam;     //idk how to make this private
    public CameraPivot cameraPivot;
    public FishingPole fishingPole;
    public GameObject interactHitbox;

    [field: Header("State")]
    [field: SerializeField]
    public PlayerState state { get; private set; }

    private PlayerState previousState;  //for MenuEventCheck()


    //Events
    private void OnEnable()
    {
        EventManager.OpenMenuEvent += OpenMenuEventCheck;   //if menu is toggled, change player state
        EventManager.CloseMenuEvent += CloseMenuEventCheck;
    }

    private void OnDisable()
    {
        EventManager.OpenMenuEvent -= OpenMenuEventCheck;   //if menu is toggled, change player state
        EventManager.CloseMenuEvent -= CloseMenuEventCheck;
    }

    private void Awake()
    {
        //Change State to Active
        ChangeState(PlayerState.Active);
    }

    void Start()
    {
        // Get handlers
        inputHandler = GetComponent<IInputHandler>();
        movementHandler = GetComponent<IMovementHandler>();
        interactHandler = GetComponent<IInteractHandler>();
        // Get components to enable/disable
        fishingPole = GetComponentInChildren<FishingPole>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == PlayerState.Active)            //player is player controlled
        {
            inputHandler.HandleInput();
            movementHandler.HandleMovement(inputHandler);
            interactHandler.HandleInteract(inputHandler);
            
        }
        else if (state == PlayerState.Inactive)     //player can't move
        {
            movementHandler.HandleMovement(null);   //gravity
        }
        else if (state == PlayerState.InMenu)
        {
            movementHandler.HandleMovement(null);   //gravity
        }
        else if (state == PlayerState.Fishing)     
        {
            inputHandler.HandleInput();
            movementHandler.HandleMovement(inputHandler);
        }
    }
    /*---State Change methods---*/
    #region State Change Methods
    public void ChangeState(PlayerState playerState)
    {
        state = playerState;

        //execute code on entering a new state
        if (state == PlayerState.Active)            //player is player controlled
        {
            //camera stuff
            EventManager.OnSwitchVCamEvent(vCam);
        }
        else if (state == PlayerState.Inactive)     //player can't move
        {
        }
        else if (state == PlayerState.InMenu)
        {
        }
        else if (state == PlayerState.Fishing)     //special form of Inactive
        {
        }

        EventManager.OnPlayerStateEvent(state);
    }

    //used by menu event
    void OpenMenuEventCheck()
    {
        if (state == PlayerState.Active) { previousState = state; ChangeState(PlayerState.InMenu); interactHitbox.SetActive(interactHitbox.activeSelf); }
    }

    void CloseMenuEventCheck()
    {
        if (state == PlayerState.InMenu) { ChangeState(previousState); interactHitbox.SetActive(interactHitbox.activeSelf); }
    }
    #endregion
}

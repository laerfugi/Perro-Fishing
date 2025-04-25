using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {Active,Inactive,Menu}

public class Player : MonoBehaviour
{
    private IInputHandler inputHandler;
    private IMovementHandler movementHandler;
    private IInteractHandler interactHandler;

    public GameObject vCam;     //idk how to make this private
    private CameraPivot cameraPivot;

    [field: Header("State")]
    [field: SerializeField]
    public PlayerState state { get; private set; }

    private PlayerState previousState;  //for MenuEventCheck()


    //Events
    private void OnEnable()
    {
        EventManager.MenuEvent += () => MenuEventCheck();   //if menu is toggled, change player state
    }

    private void OnDisable()
    {
        EventManager.MenuEvent -= () => MenuEventCheck();
    }
    

    void Start()
    {
        // Get handlers
        inputHandler = GetComponent<IInputHandler>();
        movementHandler = GetComponent<IMovementHandler>();

        // Get components to enable/disable
        interactHandler = GetComponent<IInteractHandler>();
        cameraPivot = GetComponentInChildren<CameraPivot>();

        //Change State to Active
        ChangeState(PlayerState.Active);
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
            movementHandler.HandleMovement(inputHandler); //gravity
        }
        else if (state == PlayerState.Menu)         //player and cam can't move
        {
            movementHandler.HandleMovement(inputHandler);   //gravity
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
        else if (state == PlayerState.Menu)         //player and cam can't move
        {

        }

        EventManager.OnPlayerStateEvent(state);
    }

    //used by menu event
    void MenuEventCheck()
    {
        if (state == PlayerState.Active) { previousState = state; ChangeState(PlayerState.Menu);}
        else if (state == PlayerState.Menu) { ChangeState(previousState); }
    }
    #endregion
}

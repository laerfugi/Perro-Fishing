using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {Active,Inactive,Menu}

public class Player : MonoBehaviour
{
    private IInputHandler inputHandler;
    private IMovementHandler movementHandler;

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
        }
        else if (state == PlayerState.Inactive)     //player can't move
        {
            
        }
        else if (state == PlayerState.Menu)         //player and cam can't move
        {
            
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
            cameraPivot.enabled = true;
        }
        else if (state == PlayerState.Inactive)     //player can't move
        {
            //camera stuff
            cameraPivot.enabled = true;
        }
        else if (state == PlayerState.Menu)         //player and cam can't move
        {
            //camera stuff
            cameraPivot.enabled = false;
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

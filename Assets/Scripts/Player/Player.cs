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

    [Header("State")]
    public PlayerState state;

    void Start()
    {
        // Get handlers
        inputHandler = GetComponent<IInputHandler>();
        movementHandler = GetComponent<IMovementHandler>();

        // Get components to enable/disable
        cameraPivot = GetComponentInChildren<CameraPivot>();

    }

    // Update is called once per frame
    void Update()
    {
        if (state == PlayerState.Active)            //player is player controlled
        {
            //camera stuff
            EventManager.OnSwitchVCamEvent(vCam);
            cameraPivot.enabled = true;

            //state stuff
            inputHandler.HandleInput();
            movementHandler.HandleMovement(inputHandler);
        }
        else if (state == PlayerState.Inactive)     //player can't move
        {
            //camera stuff
            cameraPivot.enabled = true;
        }
        else if (state == PlayerState.Menu)     //player can't move and can't move camera
        {
            //camera stuff
            cameraPivot.enabled = false;
        }

    }
}

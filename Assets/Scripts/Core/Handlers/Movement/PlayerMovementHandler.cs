using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementHandler : MonoBehaviour, IMovementHandler
{
    protected CharacterController characterController;
    protected Vector3 move;
    public Transform cameraPivot; // Reference to main camera
    [SerializeField] protected float yVelocity;

    // Speed
    [SerializeField] protected float speed;
    [SerializeField] protected float sprintSpeed;
    [SerializeField] protected float gravityValue;
    [SerializeField] protected float jumpHeight;

    // States
    [field: Header("States")]
    [field: SerializeField]
    public bool IsGrounded { get; private set; }
    [field: SerializeField]
    public bool IsMoving { get; private set; }
    [field: SerializeField]
    public bool IsSprinting { get; private set; }
    public bool IsFishing;  //needed for player to rotate with camera while fishing

    protected virtual void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraPivot = GetComponentInChildren<CameraPivot>().gameObject.transform;
    }

    public void HandleMovement(IInputHandler inputHandler)
    {
        StateCheck(inputHandler);

        if (inputHandler != null)
        {
            GroundCheck();
            Move(inputHandler);
            ApplyGravity();
            Jump(inputHandler);
            RotateCheck();
        }
        else
        {
            //to be used while player's inactive (disable input but still have gravity)
            GroundCheck();
            ApplyGravity();
        }
    }

    protected void GroundCheck()
    {
        IsGrounded = characterController.isGrounded;
    }

    protected void Move(IInputHandler inputHandler)
    {
        // Calculate inputs relative to camera
        Vector3 forward = cameraPivot.forward;
        Vector3 right = cameraPivot.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Get move direction
        move = inputHandler.Vertical * forward + inputHandler.Horizontal * right;
        if (move.magnitude > 1) move.Normalize();

        float currentSpeed = inputHandler.IsSprinting ? sprintSpeed : speed;
        characterController.Move(move * currentSpeed * Time.deltaTime);
    }

    protected void ApplyGravity()
    {
        if (!IsGrounded) yVelocity += gravityValue * Time.deltaTime;

        characterController.Move(new Vector3(0, yVelocity, 0));
    }

    protected void Jump(IInputHandler inputHandler)
    {
        if (inputHandler.IsJumping && IsGrounded)
        {
            yVelocity = jumpHeight;
            characterController.Move(new Vector3(0, yVelocity *Time.deltaTime, 0));
        }
    }

    // Handle player models rotation
    protected void RotateCheck()
    {
        if (IsFishing)
        {
            Vector3 temp = cameraPivot.forward;
            temp.y = 0;
            transform.forward = Vector3.Slerp(transform.forward, temp, 10 * Time.deltaTime);
        }
        else if (IsMoving)
        {
            transform.forward = Vector3.Slerp(transform.forward, move, 10 * Time.deltaTime);
        }
    }

    protected void StateCheck(IInputHandler inputHandler)
    {
        if (inputHandler != null) { IsMoving = inputHandler.Vertical != 0 || inputHandler.Horizontal != 0; } else { IsMoving = false; }
        if (inputHandler != null) { IsSprinting = inputHandler.IsSprinting; } else { IsSprinting = false; }
    }
}

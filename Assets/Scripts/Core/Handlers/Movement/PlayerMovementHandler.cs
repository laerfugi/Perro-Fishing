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

    public bool moving;
    public bool fishing;

    // States
    public bool IsGrounded { get; private set; }
    public bool IsMoving { get; private set; }
    public bool IsSprinting { get; private set; }

    protected virtual void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraPivot = GetComponentInChildren<CameraPivot>().gameObject.transform;
    }

    public void HandleMovement(IInputHandler inputHandler)
    {
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
            RotateCheck();
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

        // Handle player models rotation
        moving = inputHandler.Vertical != 0 || inputHandler.Horizontal != 0;

        if (fishing)
        {
            Vector3 temp = cameraPivot.forward;
            temp.y = 0;
            transform.forward = Vector3.Slerp(transform.forward, temp, 10 * Time.deltaTime);
        }
        else if (moving)
        {
            transform.forward = Vector3.Slerp(transform.forward, move, 10 * Time.deltaTime);
        }
        

        float currentSpeed = inputHandler.IsSprinting ? sprintSpeed : speed;
        characterController.Move(move * currentSpeed * Time.deltaTime);

        IsMoving = moving;
        IsSprinting = inputHandler.IsSprinting;
    }

    protected void ApplyGravity()
    {
        if (!IsGrounded) yVelocity += gravityValue * Time.deltaTime;
        else yVelocity = -0.25f;

        characterController.Move(new Vector3(0, yVelocity, 0));
    }

    protected void Jump(IInputHandler inputHandler)
    {
        if (inputHandler.IsJumping && IsGrounded)
        {
            yVelocity = jumpHeight;
            characterController.Move(new Vector3(0, yVelocity, 0));
        }
    }

    protected void RotateCheck()
    {
        if (fishing)
        {
            Vector3 temp = cameraPivot.forward;
            temp.y = 0;
            transform.forward = Vector3.Slerp(transform.forward, temp, 10 * Time.deltaTime);
        }
        else if (moving)
        {
            transform.forward = Vector3.Slerp(transform.forward, move, 10 * Time.deltaTime);
        }
    }
}

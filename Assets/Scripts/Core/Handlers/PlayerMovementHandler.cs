using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementHandler : MonoBehaviour, IMovementHandler
{
    private CharacterController characterController;
    private Vector3 move;
    private Transform cameraTransform; // Reference to main camera
    [SerializeField] private float yVelocity;

    // Speed
    [SerializeField] private float speed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float gravityValue;
    [SerializeField] private float jumpHeight;

    // States
    public bool IsGrounded { get; private set; }
    public bool IsMoving { get; private set; }
    public bool IsSprinting { get; private set; }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    public void HandleMovement(IInputHandler inputHandler)
    {
        GroundCheck();
        Move(inputHandler);
        ApplyGravity();
        Jump(inputHandler);
    }

    private void GroundCheck()
    {
        IsGrounded = characterController.isGrounded;
    }

    private void Move(IInputHandler inputHandler)
    {
        // Calculate inputs relative to camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Get move direction
        move = inputHandler.Vertical * forward + inputHandler.Horizontal * right;
        if (move.magnitude > 1) move.Normalize();

        // Handle player models rotation
        bool moving = inputHandler.Vertical != 0 || inputHandler.Horizontal != 0;

        if (moving)
        {
           Vector3 newDirection = Vector3.RotateTowards(transform.forward, move, 10 * Time.deltaTime, 0.0f);
           transform.rotation = Quaternion.LookRotation(newDirection);
        }

        float currentSpeed = inputHandler.IsSprinting ? sprintSpeed : speed;
        characterController.Move(move * currentSpeed * Time.deltaTime);

        IsMoving = moving;
        IsSprinting = inputHandler.IsSprinting;
    }

    private void ApplyGravity()
    {
        if (!IsGrounded) yVelocity += gravityValue * Time.deltaTime;
        else yVelocity = -0.25f;

        characterController.Move(new Vector3(0, yVelocity, 0));
    }

    private void Jump(IInputHandler inputHandler)
    {
        if (inputHandler.IsJumping && IsGrounded)
        {
            yVelocity = jumpHeight;
            characterController.Move(new Vector3(0, yVelocity, 0));
        }
    }
}

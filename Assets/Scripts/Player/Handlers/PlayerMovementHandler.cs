using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementHandler : MonoBehaviour, IMovementHandler
{
    private CharacterController characterController;
    private Vector3 move;
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
        move = inputHandler.Vertical * transform.forward + inputHandler.Horizontal * transform.right;
        if (move.magnitude > 1) move.Normalize();

        float currentSpeed = inputHandler.IsSprinting ? sprintSpeed : speed;
        characterController.Move(move * currentSpeed * Time.deltaTime);

        IsMoving = inputHandler.Vertical != 0 || inputHandler.Horizontal != 0;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundHandler : MonoBehaviour
{
    private PlayerMovementHandler playerMovementHandler;
    private PlayerInputHandler playerInputHandler;

    void Start()
    {
        playerMovementHandler = GetComponent<PlayerMovementHandler>();
        playerInputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        SoundCheck();
    }

    public void SoundCheck()
    {
        if (playerMovementHandler.IsMoving)
        {
            if (playerMovementHandler.IsSprinting)
            {
                AudioManager.Instance.PlayFullSound("Running");
            }
            else
            {
                AudioManager.Instance.PlayFullSound("Walking");
            }
        }

        if (playerInputHandler.IsJumping && playerMovementHandler.IsGrounded)
        {
            AudioManager.Instance.PlaySound("Jump");
        }
    }
}

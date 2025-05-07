using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour, IAnimationHandler
{
    public Animator animator;

    public PlayerMovementHandler playerMovementHandler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AnimationCheck();
    }

    public void AnimationCheck()
    {
        animator.SetBool("walk", playerMovementHandler.IsMoving);
        animator.SetBool("run", playerMovementHandler.IsSprinting && playerMovementHandler.IsMoving);
    } 
}

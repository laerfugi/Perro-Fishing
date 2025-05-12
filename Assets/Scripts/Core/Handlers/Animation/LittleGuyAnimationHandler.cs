using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleGuyAnimationHandler : MonoBehaviour, IAnimationHandler
{
    [Header("Cutscene Mode")]
    public bool cutsceneMode;

    public Animator animator;

    public LittleGuy littleGuy;
    public LittleGuyNav littleGuyNav;
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
        bool walk = false;
        if (littleGuy.state == LittleGuyState.Active) { walk = playerMovementHandler.IsMoving; }
        else if (littleGuy.state == LittleGuyState.AI) { walk = littleGuyNav.navMeshAgent.remainingDistance > 0.1f; }

        animator.SetBool("walk", walk);
        animator.SetBool("run", playerMovementHandler.IsSprinting && playerMovementHandler.IsMoving);


        if (cutsceneMode) animator.SetBool("walk", true);
    }


}

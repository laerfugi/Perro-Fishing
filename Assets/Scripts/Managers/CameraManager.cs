using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform[] targets;
    [SerializeField] private ThirdPersonCameraController cameraController;

    private int activeTargetIndex = 0;

    void Start()
    {
        if (targets.Length > 0)
        {
            SetActiveTarget(0);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwapTarget();
        }
    }

    public void SwapTarget()
    {
        DeactivateTarget(targets[activeTargetIndex]);
        activeTargetIndex = (activeTargetIndex + 1) % targets.Length;
        SetActiveTarget(activeTargetIndex);
    }

    private void SetActiveTarget(int index)
    {
        Transform newTarget = targets[index];
        ActivateTarget(newTarget);
        cameraController.SetTarget(newTarget);
    }

    private void DeactivateTarget(Transform target)
    {
        // Check if the target is a Player
        Player player = target.GetComponent<Player>();
        if (player != null)
        {
            player.enabled = false;
            return;
        }

        // Check if the target is a LittleGuy
        LittleGuy littleGuy = target.GetComponent<LittleGuy>();
        if (littleGuy != null)
        {
            littleGuy.SetAIControlled();
        }
    }

    private void ActivateTarget(Transform target)
    {
        // Check if the target is a Player
        Player player = target.GetComponent<Player>();
        if (player != null)
        {
            player.enabled = true;
            return;
        }

        // Check if the target is a LittleGuy
        LittleGuy littleGuy = target.GetComponent<LittleGuy>();
        if (littleGuy != null)
        {
            littleGuy.SetPlayerControlled();
        }
    }
}
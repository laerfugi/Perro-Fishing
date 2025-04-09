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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwapTarget();
        }
    }

    public void SwapTarget()
    {
        targets[activeTargetIndex].GetComponent<Player>().enabled = false;

        activeTargetIndex = (activeTargetIndex + 1) % targets.Length;

        SetActiveTarget(activeTargetIndex);
    }

    private void SetActiveTarget(int index)
    {
        targets[index].GetComponent<Player>().enabled = true;

        cameraController.SetTarget(targets[index]);
    }
}
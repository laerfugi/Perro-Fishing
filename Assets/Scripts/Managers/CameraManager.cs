using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform[] targets;

    private int activeTargetIndex = 0;

    void Start()
    {
        if (targets.Length > 0)
        {
            SetActiveCam(0);
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
        DeactivateCam(targets[activeTargetIndex]);
        activeTargetIndex = (activeTargetIndex + 1) % targets.Length;
        SetActiveCam(activeTargetIndex);
    }

    private void SetActiveCam(int index)
    {
        Transform target = targets[index];

        // Enable the virtual camera
        CinemachineVirtualCamera virtualCamera = FindVirtualCamera(target);
        if (virtualCamera != null)
        {
            virtualCamera.gameObject.SetActive(true);
        }

        EnableTarget(target);
    }

    private void DeactivateCam(Transform target)
    {
        // Disable the virtual camera
        CinemachineVirtualCamera virtualCamera = FindVirtualCamera(target);
        if (virtualCamera != null)
        {
            virtualCamera.gameObject.SetActive(false);
        }

        DisableTarget(target);
    }

    private CinemachineVirtualCamera FindVirtualCamera(Transform target)
    {
        foreach (Transform child in target)
        {
            CinemachineVirtualCamera virtualCamera = child.GetComponent<CinemachineVirtualCamera>();
            if (virtualCamera != null)
            {
                return virtualCamera;
            }
        }

        Debug.LogError($"Can't find VirtualCamera in {target.name}");
        return null;
    }

    private void DisableTarget(Transform target)
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

    private void EnableTarget(Transform target)
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
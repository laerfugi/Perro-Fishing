using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float sensitivity;
    [SerializeField] private float distance;
    [SerializeField] private float minVerticalAngle;
    [SerializeField] private float maxVerticalAngle;

    private float xRotation = 0f;
    private float yRotation = 0f;

    void LateUpdate()
    {
        if (target == null) return;
        HandleRotation();
        UpdateCameraPosition();
    }

    public void SetTarget(Transform newTarget)
    {
        CameraPivot pivot = FindObjectOfType<CameraPivot>();
        if (pivot == null)
        {
            Debug.LogError("CameraPivot not found in the scene");
        }
        target = newTarget;

        pivot.SetNewTarget(target);
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);

        cameraPivot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    private void UpdateCameraPosition()
    {
        transform.position = cameraPivot.position - cameraPivot.forward * distance;

        transform.LookAt(target);
    }
}

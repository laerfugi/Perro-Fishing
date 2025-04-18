using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivot : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    private float xRotation = 0f;
    private float yRotation = 0f;
    [SerializeField] private float minVerticalAngle;
    [SerializeField] private float maxVerticalAngle;

    void LateUpdate()
    {
        HandleRotation();
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}

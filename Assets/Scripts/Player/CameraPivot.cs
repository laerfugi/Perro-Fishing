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
    private bool isActive = true;

    float mouseX;
    float mouseY;

    private void OnEnable()
    {
        EventManager.OpenMenuEvent += toggleActive;
        EventManager.CloseMenuEvent += toggleActive;
    }
    private void OnDisable()
    {
        EventManager.OpenMenuEvent -= toggleActive;
        EventManager.CloseMenuEvent -= toggleActive;
    }


    private void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;
    }

    void LateUpdate()
    {
        if (isActive) { HandleRotation(mouseX, mouseY); }
    }

    private void HandleRotation(float mouseX, float mouseY)
    {

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    //for menus
    private void toggleActive()
    {
        isActive = !isActive;
    }
}

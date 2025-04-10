using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float sensitivity;
    private float xRotation = 0f;
    private float yRotation = 0f;
    [SerializeField] private float minVerticalAngle;
    [SerializeField] private float maxVerticalAngle;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

        cameraPivot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}

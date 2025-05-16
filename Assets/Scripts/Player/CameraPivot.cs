using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivot : MonoBehaviour
{
    public bool littleGuyMode;

    [Header("Settings")]
    [SerializeField] private float sensitivity;
    private float xRotation = 0f;
    private float yRotation = 0f;
    [SerializeField] private float minVerticalAngle;
    [SerializeField] private float maxVerticalAngle;
    [SerializeField] public bool isActive = true;

    float mouseX;
    float mouseY;

    private void OnEnable()
    {
        EventManager.OpenMenuEvent += toggleInactive;
        EventManager.CloseMenuEvent += toggleActive;

        //TEMP FIX TO ALIGN LITTLE GUY CAMERAPIVOT TO PLAYER CAMERAPIVOT AFTER BEING CRAFTED
        //doesn't perfectly rotate to player's camerapivot for some reason?
        if (littleGuyMode)
        {
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            isActive = player.cameraPivot.isActive;
            this.transform.rotation = player.cameraPivot.transform.rotation;
        }
    }
    private void OnDisable()
    {
        EventManager.OpenMenuEvent -= toggleInactive;
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
        isActive = true;
    }

    private void toggleInactive()
    {
        isActive = false;
    }

    // REPLACE LATER
    // when crafting a little guy and then closing the craft menu, it turns
    // isActive to true, so when deploying the little guy it turns isActive to false
    public void bandaid()
    {
        isActive = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CameraManager : MonoBehaviour
{
    public GameObject currentVCam;

    private void OnEnable()
    {
        EventManager.SwitchVCamEvent += switchVCam;
    }

    private void OnDisable()
    {
        EventManager.SwitchVCamEvent -= switchVCam;
    }

    private void Start()
    {

    }

    void switchVCam(GameObject vcam)
    {
        currentVCam.SetActive(false);
        currentVCam = vcam;
        currentVCam.SetActive(true);

    }
}
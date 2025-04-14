using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFacingCam : MonoBehaviour
{
    private Transform activeCameraTransform;
    void Start()
    {
        Camera activeCamera = Camera.main;
        if (activeCamera != null)
        {
            activeCameraTransform = activeCamera.transform;
        }
        else
        {
            Debug.LogError("No active camera found");
        }
    }

    void LateUpdate()
    {
        //transform.LookAt(transform.position + activeCameraTransform.forward);
        // Old cam facing implementation no difference
        //transform.forward = activeCameraTransform.forward;

        transform.forward = new Vector3(activeCameraTransform.forward.x, 0, activeCameraTransform.forward.z);       //no y axis tracking makes it look nicer
    }
}
